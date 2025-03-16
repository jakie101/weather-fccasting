# weather fccasting
# Weather Forecasting Application

This is a C# Windows Forms application that retrieves and displays current weather conditions and forecast data using the OpenWeatherMap API. The project demonstrates API integration, dynamic UI updates, error handling, unit conversion, and custom weather icon mapping.

## Features

- **Current Weather**:  
  - Fetches current weather data (temperature, humidity, wind speed, pressure, etc.).  
  - Displays a weather icon for current conditions by either loading the original PNG icon from OpenWeatherMap or by mapping to a custom icon using FontAwesome.

- **Forecast Data**:  
  - Retrieves forecast data (provided every 3 hours) and displays a summarized forecast (for example, one forecast per day) using a dynamic FlowLayoutPanel.  
  - Each forecast displays an icon, main weather description, detailed description, and the date/time.

- **Icon Mapping**:  
  - The application maps OpenWeatherMap’s icon codes (such as "01d" for a clear sky during the day or "10n" for rain at night) to custom icons using either FontAwesome or the original PNG icons.  
  - In our code, the method `GetWeatherIcon` translates the OpenWeatherMap icon code to a corresponding FontAwesome icon, while `GetWeatherColor` assigns a color based on the weather condition.  
  - For example, when the weather condition is "Clear" with an icon code "01d", `GetWeatherIcon("01d")` returns the `IconChar.Sun` and `GetWeatherColor("Clear")` returns `Color.Yellow`, so the icon is rendered in yellow.

- **Error Handling & UI Reset**:  
  - Input validation ensures that only valid city names (letters and spaces) are accepted.  
  - If a city is not found or an error occurs, the UI is reset and an error message is displayed.

- **Unit Conversion**:  
  - Temperature is converted from Kelvin to Celsius and Fahrenheit.  
  - Wind speed is converted from m/s to km/h.  
  - Pressure is converted from hPa to mmHg.

## Prerequisites

- [.NET Framework or .NET Core](https://dotnet.microsoft.com/) (depending on your project)
- Visual Studio (or your preferred C# IDE)
- An active API key from [OpenWeatherMap](https://openweathermap.org/)

