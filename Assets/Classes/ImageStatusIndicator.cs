using System;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class ImageStatusIndicator
	{

		private Texture2D fullTexture;
		private Texture2D emptyTexture;
		private Image image;

		public ImageStatusIndicator (Image _image, String full, String empty)
		{
			image = _image;
			generateTextures (image.mainTexture as Texture2D, Resources.Load(full) as Texture2D, Resources.Load(empty) as Texture2D);
		}

		private void generateTextures(Texture2D baseTex, Texture2D fullStripe, Texture2D emptyStripe){
			fullTexture = new Texture2D (baseTex.width, baseTex.height);
			emptyTexture = new Texture2D (baseTex.width, baseTex.height);


			for (int x = 0; x < baseTex.width; x++) {
				for (int y = 0; y < baseTex.height; y++) {
					Color pixel = baseTex.GetPixel (x, y);

					int stripeY = y;
					if (y > fullStripe.height) {
						stripeY = fullStripe.height - 1;
					}
					Color fullPixel = fullStripe.GetPixel (0, stripeY);

					stripeY = y;
					if (y > emptyStripe.height) {
						stripeY = emptyStripe.height - 1;
					}
					Color emptyPixel = emptyStripe.GetPixel (0, stripeY);

					if (pixel.g > 0.0f) {
						Color newFull = new Color (
							                fullPixel.r * pixel.g,
							                fullPixel.g * pixel.g,
							                fullPixel.b * pixel.g);

						fullTexture.SetPixel (x, y, newFull);

						Color newEmpty = new Color (
							                 emptyPixel.r * pixel.g,
							                 emptyPixel.g * pixel.g,
							                 emptyPixel.b * pixel.g);

						emptyTexture.SetPixel (x, y, newEmpty);
					} else {
						fullTexture.SetPixel (x, y, pixel);
						emptyTexture.SetPixel (x, y, pixel);
					}
				}
			}

			fullTexture.Apply ();
			emptyTexture.Apply ();
		}

		public void draw(float perc){
			Texture2D spriteTex = new Texture2D (image.mainTexture.width, image.mainTexture.height);

			for (int x = 0; x < Mathf.CeilToInt(spriteTex.width * perc); x++) {
				for (int y = 0; y < spriteTex.height; y++) {
					spriteTex.SetPixel (x, y, fullTexture.GetPixel (x, y));
				}
			}

			for (int x = Mathf.CeilToInt (spriteTex.width * perc); x < spriteTex.width; x++) {
				for (int y = 0; y < spriteTex.height; y++) {
					spriteTex.SetPixel (x, y, emptyTexture.GetPixel (x, y));
				}
			}



			spriteTex.Apply ();


			Sprite overrider = Sprite.Create (spriteTex, image.sprite.rect, new Vector2 (0.0f, 0.0f));
			image.overrideSprite = overrider;
		}
	}
}

