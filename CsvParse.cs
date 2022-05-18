using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace CodeLogicAssessment
{
    public class CsvParse
    {
        readonly string FileName;
        readonly CsvConfiguration Config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Encoding = Encoding.UTF8,
            Delimiter = ","
        };

        public CsvParse(string fileName)
        {
            this.FileName = fileName;
        }
        public CsvParse(string fileName, CsvConfiguration config)
        {
            this.Config = config;
            this.FileName = fileName;
        }

        public IEnumerable<Company> GetCompanies()
        {
            var companies = new List<Company>();
            try
            {
                var fs = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                var textReader = new StreamReader(fs, Encoding.UTF8);
                var csv = new CsvReader(textReader, Config);
                var data = csv.GetRecords<Record>();
                foreach (var record in data)
                {
                    //Check if company already exists, if not create a new one.
                    var company = companies.Find((c) => { return c.Name == record.Company; });

                    if (company == null && record.Company != null)
                    {
                        company = new Company(record.Company);
                        companies.Add(company);
                    }
                    //Overall Limit Population
                    if (company != null && String.IsNullOrEmpty(record.SubLimit))
                    {
                        for (int i = record.StartMonth; i <= record.EndMonth; i++)
                        {
                            //Check if month already exists, if not create a new one.
                            var existingMonth = company.Overall.Find((c) => { return c.Month == i; });
                            if(existingMonth == null)
                            {
                                company.Overall.Add(new MonthLimit(i, record.Limit));
                            }
                            else
                            {
                                existingMonth.Limit = record.Limit;
                            }
                        }
                    }
                    //SubLimit Population
                    else if (company != null && !String.IsNullOrEmpty(record.SubLimit))
                    {
                        //Check if sublimit already exists, if not create a new one.
                        List<MonthLimit> subLimit = new List<MonthLimit>();
                        if (company.SubLimits.ContainsKey(record.SubLimit))
                        {
                            subLimit = company.SubLimits[record.SubLimit];
                        }
                        else
                        {
                            company.SubLimits.Add(record.SubLimit, subLimit);
                        }
                        for (int i = record.StartMonth; i <= record.EndMonth; i++)
                        {
                            //Check if month already exists, if not create a new one.
                            var existingMonth = subLimit.Find((c) => { return c.Month == i; });
                            if( existingMonth == null)
                            {
                                subLimit.Add(new MonthLimit(i, record.Limit));
                            }
                            else
                            {
                                existingMonth.Limit = record.Limit;
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("CSV File not found.");
            }
            catch (CsvHelper.MissingFieldException ex)
            {
                Console.WriteLine("Error Reading Field from CSV file.");
                Console.WriteLine(ex.Message);
                return companies;
            }
            catch(CsvHelper.TypeConversion.TypeConverterException ex)
            {
                Console.WriteLine("Invalid field type in CSV File.");
                Console.WriteLine(ex.Message);
                return companies;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return companies;
        }

        private class Record
        {
            public string? Company { get; set; }
            public string? SubLimit { get; set; }
            public int StartMonth { get; set; }
            public int EndMonth { get; set; }
            public float Limit { get; set; }
        }
    }

}
