﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using RegExTest.Extensions;

namespace RegExTest
{
    public class KBService
    {

        public List<Error> CreateErrorsResolutionData(ErrorType processingErrorType, CancellationToken ctToken)
        {
            try
            {
                ErrorsService errService = new ErrorsService();
                if (ctToken.IsCancellationRequested)
                    ctToken.ThrowIfCancellationRequested();
                List<Error> errors = new List<Error>();

                //Get Filterd Simulation Errors list
                KBErrors errorsKB = GetFilteredKBErrors("EnergyPlus");
                //Get Simulation Error
                errors = GetErrors();
                //reduce the KB Errors by matching errors initial characters
                List<KBError> kbErrors = FilterKBList(errorsKB.KBErrorsList, errors);
                //Make string Suitable  for Regex
                ConvertKBErrorsToRegEx(kbErrors);
                errors = ParseAndSetResolutionMessage(errors, kbErrors, processingErrorType, ctToken);

                return errors;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<Error> GetErrors()
        {
            List<Error> errors = new List<Error>();
            ErrorsService errService = new ErrorsService();
            errors = errService.GetErrors(string.Format("{0}\\{1}", System.Windows.Forms.Application.StartupPath, "Errors\\Errors.xlsx"));
            foreach (Error error in errors.ToList())
            {
                if (!string.IsNullOrWhiteSpace(error.ErrorMessage))
                {
                    StringBuilder sbErrorFormatter = new StringBuilder(error.ErrorMessage);
                    sbErrorFormatter.Replace(" ** Severe  ** ", "");
                    sbErrorFormatter.Replace(" **  Fatal  ** ", "");
                    sbErrorFormatter.Replace(" ** Warning ** ", "");
                    error.ErrorMessage = sbErrorFormatter.ToString().Trim();
                }
                //simulationErrors.Add(error);
            }
            return errors;
        }


        private List<Error> ParseAndSetResolutionMessage(List<Error> errors, List<KBError> errorsKB, ErrorType processingErrorType, CancellationToken ctToken)
        {

            List<Error> distinctErrors = new List<Error>();
            Dictionary<string, Error> parsedErrorsLookup = new Dictionary<string, Error>();
            try
            {
#if DEBUG
                StringBuilder logMessage = new StringBuilder("=== Start: - List of failed simulation errors to find resolution message ===");
                logMessage.Append(Environment.NewLine);
#endif

                List<KBError> distinctKBErrors = new List<KBError>();
                if (errorsKB.Count > 0)
                    distinctKBErrors = errorsKB.GroupBy(kbErr => kbErr.ErrorMessage).Select(grp => grp.First()).ToList();

#if DEBUG
                System.Diagnostics.Stopwatch wt = new System.Diagnostics.Stopwatch();
                wt.Start();
#endif
                #region using foreach
                //                int counter = 0;
                //                foreach (KBError kbErrorInfo in distinctKBErrors)
                //                {
                //                    List<Error> sameErrorMessages = KBErrors.AsParallel().Where(error => Regex.IsMatch(error.ErrorMessage,
                //                                           kbErrorInfo.ErrorMessage, System.Text.RegularExpressions.RegexOptions.CultureInvariant)).ToList();

                //                    if (sameErrorMessages.Count > 0)
                //                    {
                //                        Error error = sameErrorMessages.First();
                //                        error.ErrorCount = sameErrorMessages.Count;
                //                        AddErrorResolutionMessage(error, kbErrorInfo);
                //                        distinctKBErrors.Add(error);
                //                        sameErrorMessages.ForEach(err => KBErrors.Remove(err));
                //                        sameErrorMessages.Clear();
                //#if DEBUG
                //                        if (error.PossibleResolution == Constants.MSG_MISSING_RESOLUTION)
                //                            logMessage.AppendLine(error.ErrorMessage);
                //#endif
                //                    }

                //                    if (IsDivisble(counter, 50))
                //                    {
                //                        if (isProgressBarShowing)
                //                            ProgressBarManager.Forward(10);
                //                    }

                //                    if (KBErrors.Count == 0) break;
                //                    counter++;
                //                }

                //                KBErrors.ForEach(error =>
                //                {
                //                    error.ErrorCount = 1;
                //                    AddErrorResolutionMessage(error, null);
                //                    distinctKBErrors.Add(error);
                //#if DEBUG
                //                    if (error.PossibleResolution == Constants.MSG_MISSING_RESOLUTION)
                //                        logMessage.AppendLine(error.ErrorMessage);
                //#endif
                //                });
                #endregion


                #region using Parallel.For

                List<KBError> warningKBErrors = distinctKBErrors.Where(kbErr => kbErr.ErrorType == "Warning").ToList();
                List<KBError> fatalKBErrors = distinctKBErrors.Where(kbErr => kbErr.ErrorType == "Fatal").ToList();
                List<KBError> severeKBErrors = distinctKBErrors.Where(kbErr => kbErr.ErrorType == "Severe").ToList();
                List<KBError> cbeccErrorKBErrors = distinctKBErrors.Where(kbErr => kbErr.ErrorType == "Error").ToList();

                //Remove All error message which should be processed
                errors.RemoveAll(error => !processingErrorType.HasFlag(error.ErrorType));

                List<Error> warningErrors = errors.Where(kbErr => kbErr.ErrorType == ErrorType.Warning).ToList();
                List<Error> fatalErrors = errors.Where(kbErr => kbErr.ErrorType == ErrorType.Fatal).ToList();
                List<Error> severeErrors = errors.Where(kbErr => kbErr.ErrorType == ErrorType.Severe).ToList();
                List<Error> cbeccErrors = errors.Where(kbErr => kbErr.ErrorType == ErrorType.Error).ToList();


                Func<List<KBError>, List<Error>, List<Error>> FindDistinctErrorMessages = (filteredKBErros, filteredErros) =>
                {
                    ConcurrentBag<Error> errorsList = new ConcurrentBag<Error>();


                    object lockObject = new object();

                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    sw.Start();


                    Parallel.For(0, filteredKBErros.Count,
                        () => new Dictionary<KBError, List<Error>>(),
                        (x, loopState, kpErrorResult) =>
                        {
#if DEBUG
                            System.Diagnostics.Stopwatch individualPatternSW = new System.Diagnostics.Stopwatch();
                            individualPatternSW.Start();
#endif
                            KBError kbError = filteredKBErros[(int)x];
                            kpErrorResult.Add(kbError, filteredErros
                                .Where(error => Regex.IsMatch(error.ErrorMessage,
                                    kbError.ErrorMessage, System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace)).ToList());
#if DEBUG
                            individualPatternSW.Stop();
                            System.Diagnostics.Debug.WriteLine(string.Format("Pattern {0} => matching Completed in {1} seconds", kbError.ErrorMessage, individualPatternSW.Elapsed.TotalSeconds));
#endif
                            return kpErrorResult;
                        },
                        (kpErrorResult) =>
                        {
                            lock (lockObject)
                            {
#if DEBUG
                                //if (kpErrorResult.Count > 0)
                                //{
                                //    foreach (KeyValuePair<KBError, List<Error>> errorResult in kpErrorResult)
                                //    {
                                //        System.Diagnostics.Debug.WriteLine(string.Format("Pattern-{0} matched {1} error messages", errorResult.Key.ErrorMessage, errorResult.Value.Count));
                                //    }
                                //}
#endif
                                foreach (KeyValuePair<KBError, List<Error>> errorResult in kpErrorResult)
                                {
                                    if (errorResult.Value.Count > 0)
                                    {
                                        Error error = null;
                                        if (errorResult.Value.Count == 1)
                                        {
                                            error = errorResult.Value.First();
                                        }
                                        else
                                        {
                                            error = new Error();
                                            error.ErrorMessage = errorResult.Value.First().ErrorMessage;
                                            error.Errors = errorResult.Value;
                                            error.ErrorType = errorResult.Value.First().ErrorType;
                                        }
                                        error.ErrorCount = errorResult.Value.Count;
                                        error.ErrorCode = errorResult.Key.ErrorCode;
                                        AddErrorResolutionMessage(error, errorResult.Key);
                                        error.ErrorMessagePattern = errorResult.Key.ErrorMessage;
                                        errorsList.Add(error);
                                        errorResult.Value.ForEach(err => errors.Remove(err));
                                    }
                                }
                            }
                        }
                        );
                    sw.Stop();
                    System.Diagnostics.Debug.WriteLine(string.Format("Completed in {0} seconds", sw.Elapsed.TotalSeconds));

                    return errorsList.ToList();

                };


                //Filter the Warning KB List
                List<KBError> filteredWarningKBList = warningKBErrors;
                List<KBError> filteredSevereKBList = severeKBErrors;
                List<KBError> filteredFatalKBList = fatalKBErrors;
                List<KBError> filteredcbeccErrorsKBList = cbeccErrorKBErrors;


                List<Task<List<Error>>> tasks = new List<Task<List<Error>>>();

                if (warningErrors.Count > 0 && (processingErrorType.HasFlag(ErrorType.Warning) || processingErrorType.Equals(ErrorType.All)))
                {
                    int equalCounts = GetMaxCommonDivisor(warningErrors.Count);
                    foreach (IEnumerable<Error> subSet in warningErrors.Split(equalCounts))
                    {
                        tasks.Add(Task.Run<List<Error>>(() => FindDistinctErrorMessages(filteredWarningKBList, subSet.ToList()), CancellationToken.None));
                    }
                }

                if (severeErrors.Count > 0 && (processingErrorType.HasFlag(ErrorType.Severe) || processingErrorType == ErrorType.All))
                {
                    int equalCounts = GetMaxCommonDivisor(severeErrors.Count);
                    foreach (IEnumerable<Error> subSet in severeErrors.Split(equalCounts))
                    {
                        tasks.Add(Task.Run<List<Error>>(() => FindDistinctErrorMessages(filteredSevereKBList, subSet.ToList()), CancellationToken.None));
                    }
                }

                if (fatalErrors.Count > 0 && (processingErrorType.HasFlag(ErrorType.Fatal) || processingErrorType.Equals(ErrorType.All)))
                {
                    int equalCounts = GetMaxCommonDivisor(fatalErrors.Count);
                    foreach (IEnumerable<Error> subSet in fatalErrors.Split(equalCounts))
                    {
                        tasks.Add(Task.Run<List<Error>>(() => FindDistinctErrorMessages(filteredFatalKBList, subSet.ToList()), CancellationToken.None));
                    }
                }

                if (cbeccErrors.Count > 0 && (processingErrorType.HasFlag(ErrorType.Error) || processingErrorType.Equals(ErrorType.All)))
                {
                    int equalCounts = GetMaxCommonDivisor(cbeccErrors.Count);
                    foreach (IEnumerable<Error> subSet in cbeccErrors.Split(equalCounts))
                    {
                        tasks.Add(Task.Run<List<Error>>(() => FindDistinctErrorMessages(filteredcbeccErrorsKBList, subSet.ToList()), CancellationToken.None));
                    }
                }

                try
                {
                    List<Error> result = new List<Error>();
                    Task.WaitAll(tasks.ToArray());
                    foreach (var task in tasks)
                    {
                        result.AddRange(task.Result);
                    }
                    result = result.Distinct().ToList();
                    result.GroupBy(res => res.ErrorMessagePattern).ToList()
                        .ForEach(grp =>
                        {
                            Error error = grp.First();
                            error.ErrorCount = grp.Sum(r => r.ErrorCount);
                            if (grp.Count() > 1)
                            {
                                grp.ToList().ForEach(grpElement =>
                                {
                                    if (grpElement != error)
                                    {
                                        if (error.Errors == null)
                                            error.Errors = new List<Error>();
                                        grpElement.ErrorCount = 1;

                                        if (grpElement.Errors != null && grpElement.Errors.Count > 0)
                                        {
                                            error.Errors.AddRange(grpElement.Errors);
                                            grpElement.Errors = null;
                                        }
                                    }
                                });
                            }
                            distinctErrors.Add(error);
                        });
                }
                finally
                {

                }

                errors.ForEach(error =>
                {
                    error.ErrorCount = 1;
                    AddErrorResolutionMessage(error, null);
                    distinctErrors.Add(error);
#if DEBUG
                    if (error.PossibleResolution == "Not Found")
                        logMessage.AppendLine(error.ErrorMessage);
#endif
                });
                #endregion

#if DEBUG
                wt.Stop();
                System.Diagnostics.Debug.WriteLine(wt.Elapsed.TotalSeconds);
#endif


            }
            catch (Exception)
            {
            }
            //if (distinctKBErrors.Count() > 0)
            //{
            //    GenerateXMLFile(distinctKBErrors, selectedSimRunResultsPath);
            //}
            return distinctErrors;

        }

        private int GetMaxCommonDivisor(int errorsCount)
        {
            int equalPartitions = 1;
            if (errorsCount <= 10)
            {
                equalPartitions = 1;
            }
            else
            {
                for (int i = 10; i < errorsCount / 2; i++)
                {
                    if (errorsCount / i < 5)
                    {
                        equalPartitions = errorsCount / i;
                        break;
                    }
                }
            }
            return equalPartitions;
        }

        /// <summary>
        /// Set resolution message in error and return resolution message
        /// </summary>
        /// <param name="error"></param>
        /// <param name="kbErrorInfo"></param>
        /// <returns></returns>
        private List<KBError> FilterKBList(List<KBError> kbList, List<Error> errorList)
        {
            List<KBError> filteredKBList = new List<KBError>();
            Parallel.ForEach(kbList, (kbErr) =>
            {
                if (!filteredKBList.Contains(kbErr))
                {
                    string message = Regex.Replace(kbErr.ErrorMessage.Trim(), @"\*\*+", @"*"); //kbErr.ErrorMessage.Trim().Replace("**", "*");
                    message = Regex.Replace(message, "  +", " ");
                    List<string> regExMsgParts = message.Split('*').ToList();
                    regExMsgParts.RemoveAll(str => string.IsNullOrWhiteSpace(str) || str.Trim().Length == 1);


                    if (errorList.Any(x => MatchPatternWithSimulationError(x.ErrorMessage, regExMsgParts)))
                    {
                        filteredKBList.Add(kbErr);
                    }
                }
            });
            return filteredKBList;
        }

        private bool MatchPatternWithSimulationError(string errorMessage, List<string> regExMsgParts)
        {
            errorMessage = Regex.Replace(errorMessage, "  +", " ");
            return regExMsgParts.All(msgPart => msgPart.Trim().Length > 1 && errorMessage.Contains(msgPart.Trim()));
        }
        /// <summary>
        /// Set resolution message in error and return resolution message
        /// </summary>
        /// <param name="error"></param>
        /// <param name="kbErrorInfo"></param>
        /// <returns></returns>
        private string AddErrorResolutionMessage(Error error, KBError kbErrorInfo)
        {
            string errorMessage = string.Empty;

            errorMessage = kbErrorInfo == null ? "Not Found" : string.IsNullOrWhiteSpace(kbErrorInfo.IssueResolution) ? "Not Found" : kbErrorInfo.IssueResolution;
            error.PossibleResolution = errorMessage;
            return errorMessage;
        }

        /// <summary>
        /// This method return KBErrors from KBErrorsKB.xml file
        /// </summary>
        /// <param name="errorSourceType"></param>
        /// <returns></returns>
        private KBErrors GetFilteredKBErrors(string errorSourceType)
        {
            KBErrors errorsKB = GetErrorsKnowledgeBase();
            errorsKB.KBErrorsList = errorsKB.KBErrorsList.Where(err => err.SourceType.Equals(errorSourceType)).ToList();
            //Make string Suitable  for Regex
            //ConvertKBErrorsToRegEx(errorsKB);
            return errorsKB;
        }

        private void ConvertKBErrorsToRegEx(List<KBError> errosKB)
        {
            for (int index = 0; index < errosKB.Count; index++)
            {
                KBError kbErrorInfo = errosKB[index];
                kbErrorInfo.ErrorMessage = CreateRegExFromKBError(kbErrorInfo.ErrorMessage);
            }
        }

        private string CreateRegExFromKBError(string kbErrorInfoMessage)
        {
            string regExString = kbErrorInfoMessage;
            if (!string.IsNullOrWhiteSpace(kbErrorInfoMessage))
            {

                StringBuilder sbResult = new StringBuilder(Regex.Escape(kbErrorInfoMessage));
                sbResult.Replace("\\*]", "\\*\\]");
                //sbResult.Replace("\\*", "[^$]*");
                //sbResult.Replace("\\*", "[^ ]*"); //Not working if space found b/w variable section
                sbResult.Replace("\\*", ".*?");

                regExString = sbResult.ToString();
                regExString = Regex.Replace(regExString, @"\\ \\ +", "\\s*?");
                regExString = Regex.Replace(regExString, string.Format("({0})+", Regex.Escape(".*?")), ".*?");
                regExString = Regex.Replace(regExString, string.Format("({0})+", Regex.Escape(".*?\\s*?")), ".*?");
                regExString = Regex.Replace(regExString, string.Format("({0})+", Regex.Escape("\\s*?")), "\\s*?");


                if (regExString.EndsWith(".*?"))
                {
                    regExString = regExString.Substring(0, regExString.Length - 1);
                }
                //regExString = regExString.StartsWith(".*?") ? regExString : regExString.Insert(0, ".*?");
            }
            return regExString;
        }

        public KBErrors GetErrorsKnowledgeBase()
        {
            KBErrors kbErrors = XMLHelper.Deserialize<KBErrors>(File.ReadAllText(string.Format("{0}\\{1}", System.Windows.Forms.Application.StartupPath, "Errors\\ErrorsKB.xml")));
            return kbErrors;
        }
    }
}
