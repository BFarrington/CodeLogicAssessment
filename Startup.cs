using System.Text.Json;

namespace CodeLogicAssessment
{
    public class Startup
    {
        static void Main(string[] args)
        {
            CsvParse csvParse = new CsvParse(@"../../../Data/InputLimits.csv");
            Output json = new(csvParse.GetCompanies());
            try
            {
                File.WriteAllText(@"../../../Data/LimitValidation.json", JsonSerializer.Serialize(json));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private class Output //Encapsulating the companies in a class so JSON output matches sample exactly.
        {
            public IEnumerable<Company> Companies { get; set; }
            public Output(IEnumerable<Company> Companies)
            {
                this.Companies = Companies;
            }
        }
    }
}
