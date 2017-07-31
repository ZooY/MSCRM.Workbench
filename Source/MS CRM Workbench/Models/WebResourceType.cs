namespace PZone.Models
{
    /// <summary>
    /// Типы веб-ресурсов.
    /// </summary>
    public enum WebResourceType
    {

        // TODO: Вынести перечисление в общую библиотеку по работе с CRM

        /// <summary>
        /// Webpage (HTML) .htm, .html
        /// </summary>
        [Image("/UI/html.png")]
        Html = 1,

        /// <summary>
        /// Style Sheet (CSS) .css
        /// </summary>
        [Image("/UI/binary.ico")]
        Css = 2,

        /// <summary>
        /// Script (JScript) .js
        /// </summary>
        [Image("/UI/js.png")]
        Js = 3,

        /// <summary>
        /// Data (XML) .xml
        /// </summary>
        [Image("/UI/xml.png")]
        Xml = 4,

        /// <summary>
        /// Image (PNG) .png
        /// </summary>
        Png = 5,

        /// <summary>
        /// Image (JPG) .jpg
        /// </summary>
        Jpg = 6,

        /// <summary>
        /// Image (GIF) .gif
        /// </summary>
        Gif = 7,

        /// <summary>
        /// Silverlight (XAP) .xap
        /// </summary>
        Xap = 8,

        /// <summary>
        /// StyleSheet (XSL) .xsl, .xslt
        /// </summary>
        Xsl = 9,

        /// <summary>
        /// Image (ICO) .ico
        /// </summary>
        Ico = 10
    }
}