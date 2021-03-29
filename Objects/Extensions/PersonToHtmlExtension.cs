using System;
using System.IO;
using System.Text;

namespace TwoLayerSolution
{
    public static class PersonToHtmlExtension
    {
        public static void PrintToHtml(this Person person)
        {
            var directory = Directory.GetCurrentDirectory();
            var htmlContent = 
                @"  <!DOCTYPE html>
                    <html>
                <head>
                <style>
                table, th, td {
                border: 1px solid black;
            }
            </style>
                </head>
                <body>
                <table style=""width:100%"">
                <tr>
                <th>ФИО</th>
                <th>Дата рождения</th>
                <th>Место рождения</th>
                <th>Номер паспорта</th>
                </tr>
                <tr>
                <td>" + person.NameSurname + @"</td>
                <td>" + person.DateOfBirth + @"</td>
                <td>" + person.BirthPlace + @"</td>
                <td>" + person.PassportNumber + @"</td>
                </tr>
                </table>

                </body>
                </html>
                ";
            using (FileStream fileStream =
                File.Open(directory + @"/Person" + person.GetHashCode(), FileMode.OpenOrCreate))
            {
                byte[] content = new UTF8Encoding(true).GetBytes(htmlContent);
                fileStream.Write(content, 0, content.Length);
                Console.WriteLine(directory);
            }
        }
    }
}