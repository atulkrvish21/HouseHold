using System.Xml.Linq;

public class XmlValidationRuleService : IXmlValidationRuleService
{
    private readonly string _filePath;

    public XmlValidationRuleService(IWebHostEnvironment env)
    {
        _filePath = Path.Combine(env.ContentRootPath, "ValidationRules.xml");
    }

   public List<string> GetAllowedValues(string fieldName)
{
    Console.WriteLine($"Reading XML for: {fieldName}");

    if (!File.Exists(_filePath))
    {
        Console.WriteLine("XML FILE NOT FOUND");
        return new List<string>();
    }

    var doc = XDocument.Load(_filePath);

    var field = doc.Descendants("Field")
                   .FirstOrDefault(f =>
                       string.Equals(
                           (string)f.Attribute("name"),
                           fieldName,
                           StringComparison.OrdinalIgnoreCase));

    if (field == null)
        return new List<string>();

    var allowedValuesAttr = (string)field.Attribute("allowedValues");

    if (string.IsNullOrWhiteSpace(allowedValuesAttr))
        return new List<string>();

    return allowedValuesAttr
        .Split(',')
        .Select(v => v.Trim())
        .ToList();
}

}
