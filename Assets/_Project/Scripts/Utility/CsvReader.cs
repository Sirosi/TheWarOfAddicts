using System;
using System.Linq;
using System.Text;

namespace TheWarOfAddicts.Utility
{
    public static class CsvReader
    {
        private const string QUOTE_CODE = "%QUOTE%";
        private const string COMMA_CODE = "%COMMA%";
        private const string ENTER_CODE = "%ENTER%";

        private static StringBuilder sb = new();


        public static string[][] Read(string csvString)
        {
            // 전처리, 큰따옴표 및 줄바꿈을 미리 정의한 코드로 변경 
            sb.Clear();
            string[] quoteSplit = csvString.Split('\"', StringSplitOptions.None);
            for (int i = 0; i < quoteSplit.Length; i++)
            {
                if (i % 2 == 1)
                {
                    quoteSplit[i] = quoteSplit[i].Replace("\n", ENTER_CODE);
                    quoteSplit[i] = quoteSplit[i].Replace(",", COMMA_CODE);
                }
                else if (i > 0 && string.IsNullOrEmpty(quoteSplit[i]))
                {
                    quoteSplit[i] = QUOTE_CODE;
                }

                sb.Append(quoteSplit[i]);
            }

            string processed = sb.ToString();


            // 전처리한 텍스트를 배열로 전환
            string[] lines = processed.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            string[][] result = lines.Select(line => line.Split(',')).ToArray();


            // 후처리, 코드로 바꿨던 문자들을 다시 재정리
            foreach (string[] strArray in result)
            {
                for (int j = 0; j < strArray.Length; j++)
                {
                    strArray[j] = strArray[j].Replace(QUOTE_CODE, "\"");
                    strArray[j] = strArray[j].Replace(COMMA_CODE, ",");
                    strArray[j] = strArray[j].Replace(ENTER_CODE, "\n");
                }
            }

            return result;
        }
    }
}