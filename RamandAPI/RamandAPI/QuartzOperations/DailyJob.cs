using Quartz;
using Serilog;

namespace RamandAPI.QuartzOperations
{
    public class DailyJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Log.Error("sup from quartz");
        }
    }
}
