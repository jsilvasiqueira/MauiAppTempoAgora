using System.Net;
using System.Text.Json;
using ModeloTempo = MauiAppTempoAgora.Tempo.Tempo;

namespace MauiAppTempoAgora;

public partial class MainPage : ContentPage
{
    private readonly HttpClient _httpClient;

    private const string ApiKey =
        "9cfc7669974b4861a4050507131a1cdb";

    public MainPage()
    {
        InitializeComponent();

        _httpClient = new HttpClient();
    }

    private async void Consultar_Clicked(
        object sender,
        EventArgs e)
    {
        try
        {
            string cidade =
                txt_cidade.Text?.Trim()
                ?? string.Empty;

            if (string.IsNullOrWhiteSpace(cidade))
            {
                await DisplayAlertAsync(
                    "Atenção",
                    "Digite o nome de uma cidade.",
                    "OK");

                return;
            }

            if (Connectivity.Current.NetworkAccess
                != NetworkAccess.Internet)
            {
                await DisplayAlertAsync(
                    "Sem conexão",
                    "Não foi possível consultar o clima porque o dispositivo está sem conexão com a internet.",
                    "OK");

                return;
            }

            activity.IsVisible = true;
            activity.IsRunning = true;

            lbl_resultado.Text = "Consultando...";

            string url =
                $"https://api.openweathermap.org/data/2.5/weather" +
                $"?q={Uri.EscapeDataString(cidade)}" +
                $"&appid={ApiKey}" +
                $"&units=metric" +
                $"&lang=pt_br";

            HttpResponseMessage resposta =
                await _httpClient.GetAsync(url);

            if (resposta.StatusCode == HttpStatusCode.NotFound)
            {
                lbl_resultado.Text = string.Empty;

                await DisplayAlertAsync(
                    "Cidade não encontrada",
                    $"Não foi possível encontrar a cidade \"{cidade}\".",
                    "OK");

                return;
            }

            if (!resposta.IsSuccessStatusCode)
            {
                lbl_resultado.Text = string.Empty;

                await DisplayAlertAsync(
                    "Erro",
                    $"A API retornou o código {(int)resposta.StatusCode}.",
                    "OK");

                return;
            }

            string json =
                await resposta.Content.ReadAsStringAsync();

            ModeloTempo tempo =
                JsonSerializer.Deserialize<ModeloTempo>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    })
                ?? new ModeloTempo();

            string descricao =
                tempo.Weather.Count > 0
                ? tempo.Weather[0].Description
                : "Não informado";

            lbl_resultado.Text =
                $"Cidade: {cidade}\n\n" +
                $"Temperatura: {tempo.Main.Temp:F1} °C\n" +
                $"Sensação térmica: {tempo.Main.Feels_Like:F1} °C\n" +
                $"Descrição: {descricao}\n" +
                $"Velocidade do vento: {tempo.Wind.Speed:F1} m/s\n" +
                $"Visibilidade: {tempo.Visibility} metros";
        }
        catch (HttpRequestException)
        {
            await DisplayAlertAsync(
                "Erro de conexão",
                "Não foi possível acessar o serviço de clima. Verifique sua conexão com a internet.",
                "OK");
        }
        catch (JsonException)
        {
            await DisplayAlertAsync(
                "Erro",
                "Não foi possível interpretar os dados recebidos da API.",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Erro",
                ex.Message,
                "OK");
        }
        finally
        {
            activity.IsVisible = false;
            activity.IsRunning = false;
        }
    }
}