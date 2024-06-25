namespace LeetCodeCSharp
{

    public class Solution_2288
    {
        //TODO:待优化
        //[TestCase("there are $1 $2 and 5$ candies in the shop", 50,  ExpectedResult = "there are $0.50 $1.00 and 5$ candies in the shop")]
        //[TestCase("1 2 $3 4 $5 $6 7 8$ $9 $10$",                100, ExpectedResult = "1 2 $0.00 4 $0.00 $0.00 7 8$ $0.00 $10$")]
        //[TestCase("$2$3 $10 $100 $1 200 $33 33$ $$ $99 $99999 $9999999999", 0,  ExpectedResult = "$2$3 $10.00 $100.00 $1.00 200 $33.00 33$ $$ $99.00 $99999.00 $9999999999.00")]
        [TestCase("1$2", 50, ExpectedResult = "1$2")]
        public string DiscountPrices(string sentence, int discount)
        {
            var sb = new StringBuilder();

            for (var i = 0 ; i < sentence.Length ; i++)
            {
                if (sentence[i] == '$')
                {
                    var j        = i + 1;
                    var isNumber = true;
                    while (j < sentence.Length && sentence[j] != ' ')
                    {
                        if (!char.IsDigit(sentence[j]))
                        {
                            isNumber = false;
                        }

                        j++;
                    }

                    if (0 == j - i - 1)
                    {
                        sb.Append('$');
                        continue;
                    }

                    var substr = sentence.Substring(i + 1, j - i - 1);

                    if (!isNumber || (i >= 1 && sentence[i - 1] != ' '))
                    {
                        sb.Append('$');
                        sb.Append(substr);
                        i = j - 1;
                        continue;
                    }


                    var price           = long.Parse(substr);
                    var discountedPrice = price * (100 - discount) / 100.0;
                    sb.Append($"${discountedPrice:F2}");
                    i = j - 1;
                }
                else
                {
                    sb.Append(sentence[i]);
                }
            }

            return sb.ToString();
        }

        [TestCase("there are $1 $2 and 5$ candies in the shop",             50,  ExpectedResult = "there are $0.50 $1.00 and 5$ candies in the shop")]
        [TestCase("1 2 $3 4 $5 $6 7 8$ $9 $10$",                            100, ExpectedResult = "1 2 $0.00 4 $0.00 $0.00 7 8$ $0.00 $10$")]
        [TestCase("$2$3 $10 $100 $1 200 $33 33$ $$ $99 $99999 $9999999999", 0,   ExpectedResult = "$2$3 $10.00 $100.00 $1.00 200 $33.00 33$ $$ $99.00 $99999.00 $9999999999.00")]
        [TestCase("1$2",                                                    50,  ExpectedResult = "1$2")]
        public string DiscountPrices2(string sentence, int discount)
        {
            var sb = new StringBuilder();

            for (var i = 0 ; i < sentence.Length ; i++)
            {
                if (sentence[i] != ' ')
                {
                    var j        = i + 1;
                    var isNumber = true;
                    while (j < sentence.Length && sentence[j] != ' ')
                    {
                        if (!char.IsDigit(sentence[j]))
                        {
                            isNumber = false;
                        }

                        j++;
                    }

                    // 非金钱的情况
                    if (sentence[i] != '$' || !isNumber || (j - i - 1 == 0))
                    {
                        var sub = sentence.Substring(i, j - i);
                        sb.Append(sub);
                        i = j - 1;
                        continue;
                    }

                    var substr = sentence.Substring(i + 1, j - i - 1);

                    var price           = long.Parse(substr);
                    var discountedPrice = price * (100 - discount) / 100.0;
                    sb.Append($"${discountedPrice:F2}");
                    i = j - 1;
                }
                else
                {
                    sb.Append(sentence[i]);
                }
            }

            return sb.ToString();
        }
    }

}