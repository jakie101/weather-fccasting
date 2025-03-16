using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Security;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.Data.SqlClient;
using static weather_fccasting.weatherForecast;
using FontAwesome.Sharp;


namespace weather_fccasting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string APIKey = "4fa22c44c6a53acbf7aa3bf681a58e38";

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            getWeather();
            getForcast();
        }
        double lon;
        double lat;
        void getWeather()
        {


            if (string.IsNullOrWhiteSpace(TBcity.Text))
            {
                MessageBox.Show("Please enter a city name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(APIKey))
            {
                MessageBox.Show("API Key is missing. Please provide a valid OpenWeatherMap API Key.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {

                using (WebClient web = new WebClient())
                {
                    string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}", TBcity.Text, APIKey);
                    var json = web.DownloadString(url);
                    weatherinfo.root info = JsonConvert.DeserializeObject<weatherinfo.root>(json);

                    IconChar weatherIcon = GetWeatherIcon(info.weather[0].icon);
                    Color iconColor = GetWeatherColor(info.weather[0].main);
                    picIcon.Image = new IconPictureBox
                    {
                        IconChar = weatherIcon,
                        IconColor = iconColor, 
                        Size = picIcon.Size,
                        BackColor = Color.Transparent
                    }.Image;

                    labCondition.Text = info.weather[0].main;
                    labDetails.Text = info.weather[0].description;
                    labSunset.Text = convertDateTime(info.sys.sunset).ToString();
                    labSunrise.Text = convertDateTime(info.sys.sunrise).ToString();

                    labWindSpeed.Text = $"{info.wind.speed * 3.6:F2} km/h"; // Convert from m/s to km/h
                    labPressure.Text = $"{info.main.pressure * 0.75006:F2} mmHg"; // Convert from hPa to mmHg

                    labTemperature.Text = $"{(info.main.temp - 273.15):F2} °C / {((info.main.temp - 273.15) * 9 / 5 + 32):F2} °F";
                    labHumidity.Text = $"{info.main.humidity}%";
                    lon = info.coord.lon;
                    lat = info.coord.lat;

                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse response && response.StatusCode == HttpStatusCode.NotFound)
                {
                    resetui();
                    MessageBox.Show("City not found. Please enter a valid city name.", "Invalid City", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                else
                {
                    MessageBox.Show("Network error: Unable to connect to the weather server. Please check your internet connection.",
                                    "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (JsonSerializationException)
            {
                MessageBox.Show("Data error: Failed to process weather data. Please try again later.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        DateTime convertDateTime(long seconds)

        {
            DateTime day = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).ToLocalTime();
            day = day.AddSeconds(seconds).ToLocalTime();

            return day;
        }
        private IconChar GetWeatherIcon(string iconCode)
        {
            switch (iconCode)
            {
                case "01d": return IconChar.Sun;
                case "01n": return IconChar.Moon;
                case "02d": return IconChar.CloudSun;
                case "02n": return IconChar.CloudMoon;
                case "03d": return IconChar.Cloud;
                case "03n": return IconChar.Cloud;
                case "04d": return IconChar.CloudMeatball;
                case "04n": return IconChar.CloudMeatball;
                case "09d": return IconChar.CloudShowersHeavy;
                case "09n": return IconChar.CloudShowersHeavy;
                case "10d": return IconChar.CloudRain;
                case "10n": return IconChar.CloudRain;
                case "11d": return IconChar.Bolt;
                case "11n": return IconChar.Bolt;
                case "13d": return IconChar.Snowflake;
                case "13n": return IconChar.Snowflake;
                case "50d": return IconChar.Smog;
                case "50n": return IconChar.Smog;
                default: return IconChar.Question; // Fallback for unknown codes
            }
        }

        private Color GetWeatherColor(string weatherCondition)
        {
            switch (weatherCondition)
            {
                case "Clear":
                    return Color.Yellow;
                case "Rain":
                    return Color.Blue;
                case "Clouds":
                    return Color.Gray;
                case "Snow":
                    return Color.White;
                case "Thunderstorm":
                    return Color.Purple;
                case "Drizzle":
                    return Color.LightBlue;
                case "Mist":
                    return Color.LightSlateGray;
                case "Fog":
                    return Color.DarkSlateGray;
                case "Haze":
                    return Color.LightGray;
                case "Smoke":
                    return Color.DimGray;
                case "Dust":
                    return Color.SandyBrown;
                case "Sand":
                    return Color.BurlyWood;
                case "Ash":
                    return Color.DarkGray;
                case "Squall":
                    return Color.DarkBlue;
                case "Tornado":
                    return Color.Maroon;
                default:
                    return Color.White; // Fallback color
            }
        }


        private void picIcon_Click(object sender, EventArgs e)
        {

        }
        double minutes;
        void getForcast()
        {
            using (WebClient web = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&exclude=currently,minutely,hourly,alerts&appid={2}", lat, lon, APIKey);
                var json = web.DownloadString(url);
                weatherForecast.ForecastInfo ForecastInfo = JsonConvert.DeserializeObject<weatherForecast.ForecastInfo>(json);
                ForecastUC FUC;

                for (int i = 0; i < 20; i++)
                {
                    FUC = new ForecastUC();
                    IconChar weatherIcon = GetWeatherIcon(ForecastInfo.list[i].weather[0].icon);
                    Color iconColor = GetWeatherColor(ForecastInfo.list[i].weather[0].icon);
                    FUC.picWeatherIcon.Image = new IconPictureBox
                    {
                        IconChar = weatherIcon,
                        IconColor = iconColor, 
                        Size = FUC.picWeatherIcon.Size,
                        BackColor = Color.Transparent
                    }.Image;


                    FUC.labMainWeather.Text = ForecastInfo.list[i].weather[0].main;
                    FUC.labWeatherDescription.Text = ForecastInfo.list[i].weather[0].description;
                    FUC.labDT.Text = convertDateTime(ForecastInfo.list[i].dt).DayOfWeek.ToString();



                    FLP.Controls.Add(FUC);

                }

            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TBcity_TextChanged(object sender, EventArgs e)
        {

        }
        private void resetui()
        {
            FLP.Controls.Clear();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\JAKES\source\repos\weather fccasting\weather fccasting\weather.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO weather (city, condition, details, windspeed, pressure, temperature, humidity) VALUES (@city, @condition, @details, @windspeed, @pressure, @temperature, @humidity)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@city", TBcity.Text);
                    command.Parameters.AddWithValue("@condition", labCondition.Text);
                    command.Parameters.AddWithValue("@details", labDetails.Text);
                    command.Parameters.AddWithValue("@windspeed", labWindSpeed.Text);
                    command.Parameters.AddWithValue("@pressure", labPressure.Text);
                    command.Parameters.AddWithValue("@temperature", labTemperature.Text);
                    command.Parameters.AddWithValue("@humidity", labHumidity.Text);

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data saved successfully");
                    }
                    else
                    {
                        MessageBox.Show("Failed to save data");
                    }
                }
            }
        }
    }
}
