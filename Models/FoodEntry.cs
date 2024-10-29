//Класс для хранения данных о продукте (название, калории, белки, жиры, углеводы, время приема пищи)
namespace Food_Diary.Models
{
    public class FoodEntry
    {
        public string Name { get; set; }
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbohydrates { get; set; }
        public string MealType { get; set; }
        public DateTime Date { get; set; }
    }
}