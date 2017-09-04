using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegExTest
{
    class Program
    {
        static void Main()
        {
            KBService service = new KBService();
            CancellationTokenSource ctSource = new CancellationTokenSource();

            try
            {
                Task<List<Error>> getReferencesTask = Task.Factory.StartNew<List<Error>>(() =>
                       service.CreateErrorsResolutionData(ErrorType.All, ctSource.Token)
                      , ctSource.Token);

                getReferencesTask.Wait();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                
            }
           
        }
    }
}
