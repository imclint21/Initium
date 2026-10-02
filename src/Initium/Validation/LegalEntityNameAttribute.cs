using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Initium.Validation;

/// <summary>
/// Validates that a company name ends with a recognized legal-form suffix (Inc, LLC, Ltd, SARL, GmbH, …).
/// The suffix set is broadly international; multi-word forms are listed before their shorter variants so they
/// win, and the match is after a space/comma/period, case-insensitive, with an optional trailing period
/// (<c>Acme Inc</c>, <c>Acme Inc.</c>, <c>Acme, LLC</c>). A null/empty/whitespace value passes (the field is
/// optional). This checks the shape only — it does not verify that the company is actually registered.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class LegalEntityNameAttribute : ValidationAttribute
{
	private static readonly Regex SuffixPattern = new(
		@"[\s,.](" + string.Join("|",
			"Co\\., Ltd", "Pvt\\.? Ltd", "Pte\\.? Ltd", "Sdn\\.? Bhd", "sp\\.? z o\\.?o", "d\\.o\\.o", "s\\.r\\.o",
			"a\\.s", "L\\.L\\.C", "K\\.K",
			"Incorporated", "Corporation", "Limited", "Company", "Holdings", "Group",
			"Inc", "LLC", "Ltd", "Corp", "PLC", "LLP", "LP", "Co", "Pvt", "Pte", "Bhd",
			"SARL", "SASU", "EURL", "SPRL", "SAS", "SCI", "SA", "SCA", "SNC",
			"GmbH", "KGaA", "OHG", "AG", "UG", "KG", "eG", "SE",
			"BV", "NV", "CV",
			"SPA", "SRLS", "SRL", "SLU", "SLNE", "SL", "Lda",
			"Oyj", "Oy", "ASA", "AS", "ApS", "AB", "UAB", "SIA",
			"OOO", "PAO", "Kft", "Zrt", "Nyrt",
			"PJSC", "FZCO", "FZE", "Ltda", "KK"
		) + @")\.?$",
		RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

	/// <inheritdoc />
	public override bool IsValid(object? value)
	{
		if (value is not string name || string.IsNullOrWhiteSpace(name))
			return true; // optional

		return SuffixPattern.IsMatch(name.Trim());
	}

	/// <inheritdoc />
	public override string FormatErrorMessage(string name) =>
		"Enter a real registered company name ending with its legal form (e.g. Acme Inc, Acme LLC, Acme SARL), or leave it empty.";
}
