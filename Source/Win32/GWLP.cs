namespace Win32
{
    public enum GWLP
    {
        /// <summary>
        /// Retrieves the user data associated with the window. 
        /// This data is intended for use by the application that created the window. 
        /// Its value is initially zero.
        /// </summary>
        USERDATA = -21,

        /// <summary>
        /// Retrieves the extended window styles.
        /// </summary>
        EXSTYLE = -20,

        /// <summary>
        /// Retrieves the window styles.
        /// </summary>
        STYLE = -16,

        /// <summary>
        /// Retrieves the identifier of the window.
        /// </summary>
        ID = -12,

        /// <summary>
        /// Retrieves a handle to the parent window, if there is one.
        /// </summary>
        HWNDPARENT = -8,

        /// <summary>
        /// Retrieves a handle to the application instance.
        /// </summary>
        HINSTANCE = -6,

        /// <summary>
        /// Retrieves the pointer to the window procedure, 
        /// or a handle representing the pointer to the window procedure. 
        /// You must use the CallWindowProc function to call the window procedure.
        /// </summary>
        WNDPROC = -4
    }
}