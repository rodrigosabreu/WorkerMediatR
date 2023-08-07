namespace app.Domain.Entities;

public class TimeAtendimentoPrivate
{
    public string Cliente { get; set; }
    public Time TimeAtendimento { get; set; }
}

public class Time
{
    public string Banker { get; set; }
    public string Gerente { get; set; }
    public string Especialista { get; set; }
}
