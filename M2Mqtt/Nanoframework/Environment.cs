/*
Contributors:   
   .NET Foundation and Contributors - nanoFramework support
*/

namespace System
{
    static class Environment
    {
        public static int TickCount
        {
            get { return (int)DateTime.UtcNow.Ticks; }
        }
    }
}
