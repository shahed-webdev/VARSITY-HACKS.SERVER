using VARSITY_HACKS.DATA;

namespace VARSITY_HACKS.BusinessLogic;

public static class SuggestedStudyRuleClass
{   
    public static int EventBreakDuration { get; } = 30;
    public static int GetEventDateDuration(PersonalityType type, DifficultyLevel difficulty)
    {
        switch (type)
        {
            case PersonalityType.EarlyBird:
                return difficulty switch
                {
                    DifficultyLevel.Low => 60,
                    DifficultyLevel.Moderate => 90,
                    DifficultyLevel.High => 120,
                    _ => 120
                };

            case PersonalityType.NightOwl:
                return difficulty switch
                {
                    DifficultyLevel.Low => 60,
                    DifficultyLevel.Moderate => 90,
                    DifficultyLevel.High => 120,
                    _ => 120
                };

            case PersonalityType.WeekendWarrior:
                default:
                return difficulty switch
                {
                    DifficultyLevel.Low => 30,
                    DifficultyLevel.Moderate => 60,
                    DifficultyLevel.High => 90,
                    _ => 90
                };
        }
    }
    public static int GetWeekendDuration(PersonalityType type, DifficultyLevel difficulty)
    {
        switch (type)
        {
            case PersonalityType.EarlyBird:
                return difficulty switch
                {
                    DifficultyLevel.Low => 90,
                    DifficultyLevel.Moderate => 120,
                    DifficultyLevel.High => 150,
                    _ => 150
                };

            case PersonalityType.NightOwl:
                return difficulty switch
                {
                    DifficultyLevel.Low => 90,
                    DifficultyLevel.Moderate => 120,
                    DifficultyLevel.High => 150,
                    _ => 150
                };

            case PersonalityType.WeekendWarrior:
            default:
                return difficulty switch
                {
                    DifficultyLevel.Low => 90,
                    DifficultyLevel.Moderate => 150,
                    DifficultyLevel.High => 180,
                    _ => 180
                };
        }
    }

    public static TimeSpan WeekdayStarTime(PersonalityType type)
    {
        return type switch
        {
            PersonalityType.EarlyBird => TimeSpan.Parse("16:00"),
            PersonalityType.NightOwl => TimeSpan.Parse("18:00"),
            PersonalityType.WeekendWarrior => TimeSpan.Parse("16:00"),
            _ => TimeSpan.Parse("16:00")
        };
    }

    public static TimeSpan WeekendStarTime(PersonalityType type)
    {
        return type switch
        {
            PersonalityType.EarlyBird => TimeSpan.Parse("9:00"),
            PersonalityType.NightOwl => TimeSpan.Parse("17:00"),
            PersonalityType.WeekendWarrior => TimeSpan.Parse("9:00"),
            _ => TimeSpan.Parse("9:00")
        };
    }
    public static int WeekdayDurationMinute(PersonalityType type)
    {
        return type switch
        {
            PersonalityType.EarlyBird => 540,
            PersonalityType.NightOwl => 540,
            PersonalityType.WeekendWarrior => 540,
            _ => 540
        };
    }  
    public static int WeekendDurationMinute(PersonalityType type)
    {
        return type switch
        {
            PersonalityType.EarlyBird => 480,
            PersonalityType.NightOwl => 450,
            PersonalityType.WeekendWarrior => 300,
            _ => 300
        };
    }
    
}