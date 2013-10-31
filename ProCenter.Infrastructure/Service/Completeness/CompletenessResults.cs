namespace ProCenter.Infrastructure.Service.Completeness
{
    public class CompletenessResults
    {
        public CompletenessResults(string completenessCategory, int total, int numbercomplete)
        {
            CompletenessCategory = completenessCategory;
            Total = total;
            NumberComplete = numbercomplete;
        }

        public string CompletenessCategory { get; private set; }

        public int Total { get; private set; }

        public int NumberComplete { get; private set; }

        public int NumberIncomplete
        {
            get { return Total - NumberComplete; }
        }

        public double PercentComplete
        {
            get { return (double) NumberComplete/Total; }
        }

        public void UpdateTotal(int total)
        {
            Total = total;
            if (Total < NumberComplete)
            {
                NumberComplete = Total;
            }
        }
    }
}