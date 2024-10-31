//Класс для хранения данных о продукте (название, калории, белки, жиры, углеводы, время приема пищи)
namespace Дневник_Питания.Core.Models 

{
    public class Food
    {
        public string Name { get; set; }
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbohydrates { get; set; }
        public string MealTime { get; set; }
        public DateTime Date { get; set; } // Дата, когда еда была съедена

    }
}