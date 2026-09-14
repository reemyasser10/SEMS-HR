namespace HR.Application.Common.Interfaces;

public interface ILocalizationService
{
    string Get(string key, params object[] args);
    string this[string key] { get; }
}
