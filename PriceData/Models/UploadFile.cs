using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace PriceData.Models
{
	public class UploadFile : IValidatableObject
	{
		[Required(ErrorMessage = "Please select a file.")]
		[DataType(DataType.Upload)]
		public IFormFile File { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			List<ValidationResult> results = new List<ValidationResult>();

			var file = ((UploadFile)validationContext.ObjectInstance).File;
			var ext = Path.GetExtension(file.FileName);
			var size = file.Length;

			if (!ext.ToLower().Equals(".csv")) 
			{
				results.Add(new ValidationResult("File extension is not valid."));
				return results;
			}

			if (size > (2 * 1024 * 1024)) 
			{
				results.Add(new ValidationResult("File size is larger than 2MB."));
				return results;
			}

			using (var reader = new StreamReader(file.OpenReadStream()))
			{
				int row = 0;

				while (reader.Peek() >= 0)
				{
					row += 1;
					var line = reader.ReadLine();
					// Ignore headers 
					if (row == 1) continue;

					string[] parts = line.Split(',');

					if (parts?.Length != 2)
					{
						results.Add(new ValidationResult($"{parts.Length} separators on line {row}"));
						return results;
					}

					DateTime dateTime;

					if (!DateTime.TryParse(parts[0], out dateTime))
					{
						results.Add(new ValidationResult($"Invalid Date on line {row}"));
						return results;
					}

					decimal value;

					if (!decimal.TryParse(parts[1], out value))
					{
						results.Add(new ValidationResult($"Invalid Price on line {row}"));
						return results;
					}
				}
			}

			return results;
		}
	}
}
