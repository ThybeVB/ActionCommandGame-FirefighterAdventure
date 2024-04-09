using ActionCommandGame.Services.Helpers;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Extensions
{
    public static class PlayerExtensions
    {
        public static int GetLevel(this PlayerResult player)
        {
            return PlayerLevelHelper.GetLevelFromExperience(player.Experience);
        }

        public static int GetExperienceForNextLevel(this PlayerResult player)
        {
            return PlayerLevelHelper.GetExperienceForNextLevel(player.Experience);
        }

        public static int GetLevelFromExperience(this PlayerResult player)
        {
            return PlayerLevelHelper.GetLevelFromExperience(player.Experience);
        }

        public static int GetRemainingExperienceUntilNextLevel(this PlayerResult player)
        {
            return PlayerLevelHelper.GetRemainingExperienceUntilNextLevel(player.Experience);
        }
    }
}
