namespace MauiAppTempoAgora.Tempo
{
    public class Tempo
    {
        public Main Main { get; set; } = new Main();
        public Wind Wind { get; set; } = new Wind();
        public List<Weather> Weather { get; set; } = new List<Weather>();
        public int Visibility { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
        public double Feels_Like { get; set; }
        public double Temp_Min { get; set; }
        public double Temp_Max { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
    }

    public class Wind
    {
        public double Speed { get; set; }
    }

    public class Weather
    {
        public string Description { get; set; } = string.Empty;
    }
}