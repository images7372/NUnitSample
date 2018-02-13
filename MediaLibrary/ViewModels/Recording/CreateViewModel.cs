using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaLibrary.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MediaLibrary.ViewModels.Recording
{
    [CustomValidation(typeof(CreateViewModel), "IsValidDetails")]
    public class CreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "タイトルに入力が必要です")]
        [Display(Name = "タイトル")]
        public string Title { get; set; }

        [Display(Name = "リリース日")]
        public DateTime? ReleaseDate { get; set; }

        [Display(Name = "レーベル")]
        public IEnumerable<SelectListItem> Labels { get; set; }

        [Display(Name = "アーティスト")]
        public IEnumerable<SelectListItem> Artists { get; set; }
        public int? SelectedLabelId { get; set; }
        public int? SelectedArtistId { get; set; }

        [Display(Name = "曲名")]
        public List<string> TrackTitles { get; set; }

        [Display(Name = "演奏時間（秒）")]
        public List<int?> Durations { get; set; }

        public static ValidationResult IsValidDetails(CreateViewModel vm)
        {
            if (vm.TrackTitles.Count != vm.Durations.Count)
            {
                throw new InvalidOperationException("通常操作ではあり得ない値が渡された");
            }

            foreach (var i in Enumerable.Range(0, vm.TrackTitles.Count))
            {
                if (string.IsNullOrWhiteSpace(vm.TrackTitles[i]) || vm.Durations[i] == null)
                {
                    return new ValidationResult("曲名または演奏時間に入力がありません");
                }

            }
            return ValidationResult.Success;
        }
    }
}