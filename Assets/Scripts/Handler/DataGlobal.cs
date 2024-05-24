using CommandChoice.Model;

namespace CommandChoice.Data
{
    class DataGlobal
    {
        public static DataGamePlay GamePlay { get; private set; } = new();
        public static LevelSceneDataModel LevelScene { get; private set; } = new();
        public static SettingGame SettingGame { get; private set; } = new();
        public const int minusScoreBoxCommand = 2;
        public const int minusScoreLostMail = 30;
        public const float timeDeray = 1f;
        public const int HpDefault = 3;
        public const int MailMax = 3;
        public const int MailDefault = 0;
        public const int ScoreDefault = 150;

        public static void ImportLevelScene()
        {
            GamePlay = new();
            SettingGame.LoadSettings();
        }
    }
}
