using System;
using System.Diagnostics;
using CSEngine.Shared;

namespace CSEngine.Client
{
    public class G
    {
        public static CSEngineApp CSEngineApp { get; set; }
        public static ClientLogic ClientLogic { get; set; }
        public static ClientPlayerManager ClientPlayerManager { get; set; }
        public static ClientPlayerInput ClientPlayerInput { get; set; }
    }
}