using Avalonia.Media.Imaging;
using Avalonia.MusicStore.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.MusicStore.ViewModels
{
    public class AlbumViewModel : ViewModelBase
    {

        private Bitmap? _cover;
        private readonly Album _album;

        public AlbumViewModel(Album album)
        {
            _album = album;
        }

        public string Artist => _album.Artist;

        public string Title => _album.Title;


        public Bitmap? Cover
        {
            get => _cover;
            private set => this.RaiseAndSetIfChanged(ref _cover, value);
        }

        public async Task LoadCover()
        {
            await using (var ImageStream = await _album.LoadCoverBitmapAsync())
            {
                Cover = await Task.Run(() => Bitmap.DecodeToWidth(ImageStream, 400));
            }
        }

        public async Task SaveToDiskAsync()
        {
            await _album.SaveAsync();

            if (Cover != null)
            {
                var bitmap = Cover;

                await Task.Run(() =>
                {
                    using (var fs = _album.SaveCoverBitmapStream())
                    {
                        bitmap.Save(fs);
                    }
                });
            }
        }

    }
}
