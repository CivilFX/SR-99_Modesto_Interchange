
namespace CivilFX
{
    public enum Phase
    {
        //None = 0,
        Existing = 1,           // 000000000000000000001
        Now = 2,                // // 000000000000000000010

        Phase1 = 4,      // 000000000000000000100
        Phase2 = 8,      // 000000000000000001000
        Phase3 = 16,     // 000000000000000010000
        Phase4 = 32,     // 000000000000000100000
        Phase5 = 64,     // 000000000000001000000
        Phase6 = 128,    // 000000000000010000000
        Phase7 = 256,    // 000000000000100000000
        Phase8 = 512,    // 000000000001000000000
        Phase9 = 1024,   // 000000000010000000000
        Phase10 = 2048,   // 000000000100000000000
        Phase11 = 4096,   // 000000001000000000000
        Phase12 = 8192,   // 000000010000000000000
        Phase13 = 16384,  // 000000100000000000000
        Phase14 = 32768,  // 000001000000000000000
        Phase15 = 65536,  // 000010000000000000000
        Phase16 = 131072, // 000100000000000000000
        Phase17 = 262144, // 001000000000000000000
        Phase18 = 524288, // 010000000000000000000

        Ultimate = 1048576,  // 100000000000000000000
    }

    public class PhaseManager
    {
        public delegate void PhaseShift(Phase newPhase);
        public static PhaseShift PhaseShifted;

        private static PhaseManager instance = null;
        private static Phase _currentPhase;

        public static PhaseManager GetInstance()
        {
            if (instance == null)
            {
                instance = new PhaseManager();
                _currentPhase = Phase.Existing;
            }

            return instance;
        }

        public static Phase CurrentPhase
        {
            get { return _currentPhase; }
            private set { }
        }

        public static void RaisePhaseShift(Phase newPhase)
        {
            if (PhaseShifted != null)
                PhaseShifted(newPhase);

            _currentPhase = newPhase;
        }
    }
}