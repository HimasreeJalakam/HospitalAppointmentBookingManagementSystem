namespace Models.Helper
{
    public class Dtype
    {
        public static List<string> DtypeList => new List<string>
        {
            "Hypertension",
            "Diabetes Mellitus",
            "Asthma",
            "COPD",
            "Coronary Artery Disease",
            "Heart Failure",
            "Stroke",
            "Epilepsy",
            "Migraine",
            "Depression",
            "Anxiety",
            "Schizophrenia",
            "Bipolar Disorder",
            "Osteoarthritis",
            "Rheumatoid Arthritis",
            "Gout",
            "Tuberculosis",
            "Hepatitis B",
            "Hepatitis C",
            "HIV/AIDS",
            "UTI",
            "Pneumonia",
            "Bronchitis",
            "GERD",
            "Peptic Ulcer",
            "IBS",
            "Crohn's Disease",
            "Ulcerative Colitis",
            "Anemia",
            "Thyroid Disorder"
        };
        public static void Validate(string dtype)
        {
            if (!Dtype.DtypeList.Contains(dtype))
            {
                throw new ArgumentException($"Invalid Dtype: '{dtype}'. Must be one of the predefined diagnoses.");
            }
        }
    }
}
