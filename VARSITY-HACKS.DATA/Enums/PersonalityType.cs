using System.ComponentModel;

namespace VARSITY_HACKS.DATA;

public enum PersonalityType
{
    [Description("Early Bird")] EarlyBird = 1,
    [Description("Night Owl")] NightOwl,
    [Description("Weekend Warrior")] WeekendWarrior
}