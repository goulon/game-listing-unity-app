using System.Collections;
using UnityEngine.Networking;
namespace UnityEngine.Replay
{
    /// <summary>
    ///     Abstract UI object that contains text and images and a reference to a content Listing
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class UIEntity : MonoBehaviour
    {
        protected CanvasGroup m_CanvasGroup;
        protected UIImage[] m_Images;
        protected Listing m_Listing;
        protected RectTransform m_RectTransform;
        protected UIText[] m_Texts;

        /// <summary>
        ///     Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
            m_RectTransform = GetComponent<RectTransform>();
            m_Texts = GetComponentsInChildren<UIText>();
            m_Images = GetComponentsInChildren<UIImage>();
        }

        /// <summary>
        ///     Gets the image content based on a URL
        /// </summary>
        /// <param name="image">The image to set the texture to</param>
        /// <param name="imageUrl">The image URL to request the texture</param>
        private IEnumerator GetImageTexture(UIImage image, string imageUrl)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imageUrl))
            {
                yield return uwr.SendWebRequest();
                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(uwr.error);
                }
                else
                {
                    // Get downloaded asset bundle
                    image.SetImage(DownloadHandlerTexture.GetContent(uwr));
                }
            }
        }

        /// <summary>
        ///     Sets the entity's content based on a Listing
        /// </summary>
        /// <param name="l">The Listing to use</param>
        public virtual void SetData(Listing l)
        {
            m_Listing = l;
            foreach (var text in m_Texts) text.SetText(l.GetText(text.textType));
            foreach (var image in m_Images)
            {
                string imageUrl = m_Listing.images[0].url;
                StartCoroutine(GetImageTexture(image, imageUrl));
            }
        }
    }
}
