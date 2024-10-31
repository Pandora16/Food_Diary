//Класс для хранения данных о пользователе (рост, вес, возраст, пол, уровень активности, BMR, цель по каллориям)
namespace Дневник_Питания.Core.Models
{
    public class User
    {
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string ActivityLevel { get; set; }
        public double BMR { get; set; }
        public double TargetCalories { get; set; }
        
    }
}