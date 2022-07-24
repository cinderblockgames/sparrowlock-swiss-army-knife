using System.Runtime.Versioning;
using System.Text;
using Underlying = System.IO.File;

namespace SLSAK.Docker.IO;

/// <summary>
/// Wraps <see cref="System.IO.File" /> and uses a temporary file returned by <see cref="System.IO.Path.GetTempFileName" />
/// to get around weirdness when running in a container.  (Writing directly to a file in a
/// volume throws an access denied exception and leaves the file blank, even if you have access
/// to the location, as evidenced by the fact that copying or moving the file works.)
/// 
/// This is a drop-in replacement for <see cref="System.IO.File" />; any methods that don't have fixes call
/// the underlying <see cref="System.IO.File" /> method directly.
/// </summary>
public static class File
{

    #region " System.IO.File Wrappers "

    /// <summary>
    /// Appends lines to a file, and then closes the file. If the specified file does
    /// not exist, this method creates a file, writes the specified lines to the file,
    /// and then closes the file.
    /// </summary>
    /// <param name="path">
    /// The file to append the lines to. The file is created if it doesn't already exist.
    /// </param>
    /// <param name="contents">
    /// The lines to append to the file.
    /// </param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is a zero-length string, contains only white space, or contains one more
    /// invalid characters defined by the <see cref="System.IO.Path.InvalidPathChars" /> method.
    /// </exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// Either <paramref name="path" /> or <paramref name="contents" /> is null.
    /// </exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid (for example, the directory doesn't exist or it is on an unmapped
    /// drive).
    /// </exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// The file specified by <paramref name="path" /> was not found.
    /// </exception>
    /// <exception cref="T:System.IO.IOException">
    /// An I/O error occurred while opening the file.
    /// </exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="path" /> exceeds the system-defined maximum length.
    /// </exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.
    /// </exception>
    /// <exception cref="T:System.Security.SecurityException">
    /// The caller does not have permission to write to the file.
    /// </exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <paramref name="path" />specifies a file that is read-only. -or- This operation is not supported
    /// on the current platform. -or- <paramref name="path" /> is a directory.
    /// </exception>
    /// <remarks>
    /// Uses temp file.
    /// </remarks>
    public static void AppendAllLines(string path, IEnumerable<string> contents)
    {
        WithTempFile(temp => Underlying.AppendAllLines(temp, contents), path);
    }

    /// <summary>
    /// Appends lines to a file by using a specified encoding, and then closes the file.
    /// If the specified file does not exist, this method creates a file, writes the
    /// specified lines to the file, and then closes the file.
    /// </summary>
    /// <param name="path">
    /// The file to append the lines to. The file is created if it doesn't already exist.
    /// </param>
    /// <param name="contents">
    /// The lines to append to the file.
    /// </param>
    /// <param name="encoding">
    /// The character encoding to use.
    /// </param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is a zero-length string, contains only white space, or contains one more
    /// invalid characters defined by the <see cref="System.IO.Path.InvalidPathChars" /> method.
    /// </exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// Either <paramref name="path" />, <paramref name="contents" />, or <paramref name="encoding" /> is null.
    /// </exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// <paramref name="path" /> is invalid (for example, the directory doesn't exist or it is on an unmapped
    /// drive).
    /// </exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// The file specified by <paramref name="path" /> was not found.
    /// </exception>
    /// <exception cref="T:System.IO.IOException">
    /// An I/O error occurred while opening the file.
    /// </exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="path" /> exceeds the system-defined maximum length.
    /// </exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> is in an invalid format.
    /// </exception>
    /// <exception cref="T:System.Security.SecurityException">
    /// The caller does not have permission to write to the file.
    /// </exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <paramref name="path" />specifies a file that is read-only. -or- This operation is not supported
    /// on the current platform. -or- <paramref name="path" /> is a directory. -or- The caller does not have
    /// the required permission.
    /// </exception>
    /// <remarks>
    /// Uses temp file.
    /// </remarks>
    public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
    {
        WithTempFile(temp => Underlying.AppendAllLines(temp, contents, encoding), path);
    }

    /// <summary>
    /// Asynchronously appends lines to a file by using a specified encoding, and then
    /// closes the file. If the specified file does not exist, this method creates a
    /// file, writes the specified lines to the file, and then closes the file. 
    /// </summary>
    /// <param name="path">
    /// The file to append the lines to. The file is created if it doesn't already exist.
    /// </param>
    /// <param name="contents">
    /// The lines to append to the file.
    /// </param>
    /// <param name="encoding">
    /// The character encoding to use.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="System.Threading.CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous append operation.
    /// </returns>
    /// <remarks>
    /// Uses temp file.
    /// </remarks>
    public static Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        return WithTempFile(temp => Underlying.AppendAllLinesAsync(temp, contents, encoding, cancellationToken), path);
    }

    /// <summary>
    /// Asynchronously appends lines to a file, and then closes the file. If the specified
    /// file does not exist, this method creates a file, writes the specified lines to
    /// the file, and then closes the file. 
    /// </summary>
    /// <param name="path">
    /// The file to append the lines to. The file is created if it doesn't already exist.
    /// </param>
    /// <param name="contents">
    /// The lines to append to the file.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="System.Threading.CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous append operation.
    /// </returns>
    /// <remarks>
    /// Uses temp file.
    /// </remarks>
    public static Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
    {
        return WithTempFile(temp => Underlying.AppendAllLinesAsync(temp, contents, cancellationToken), path);
    }

    //
    // Summary:
    //     Opens a file, appends the specified string to the file, and then closes the file.
    //     If the file does not exist, this method creates a file, writes the specified
    //     string to the file, then closes the file.
    //
    // Parameters:
    //   path:
    //     The file to append the specified string to.
    //
    //   contents:
    //     The string to append to the file.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, the directory doesn't exist or it
    //     is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only. -or- This operation is not supported
    //     on the current platform. -or- path specified a directory. -or- The caller does
    //     not have the required permission.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    public static void AppendAllText(string path, string? contents)
    {
        WithTempFile(temp => Underlying.AppendAllText(temp, contents), path);
    }

    //
    // Summary:
    //     Appends the specified string to the file using the specified encoding, creating
    //     the file if it does not already exist.
    //
    // Parameters:
    //   path:
    //     The file to append the specified string to.
    //
    //   contents:
    //     The string to append to the file.
    //
    //   encoding:
    //     The character encoding to use.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, the directory doesn't exist or it
    //     is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only. -or- This operation is not supported
    //     on the current platform. -or- path specified a directory. -or- The caller does
    //     not have the required permission.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    public static void AppendAllText(string path, string? contents, Encoding encoding)
    {
        WithTempFile(temp => Underlying.AppendAllText(temp, contents, encoding), path);
    }

    /// <summary>
    /// Asynchronously opens a file or creates the file if it does not already exist,
    /// appends the specified string to the file using the specified encoding, and then
    /// closes the file.
    /// </summary>
    /// <param name="path">
    /// The file to append the specified string to.
    /// </param>
    /// <param name="contents">
    /// The string to append to the file.
    /// </param>
    /// <param name="encoding">
    /// The character encoding to use.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="System.Threading.CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous append operation.
    /// </returns>
    /// <remarks>
    /// Uses temp file.
    /// </remarks>
    public static Task AppendAllTextAsync(string path, string? contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        return WithTempFile(temp => Underlying.AppendAllTextAsync(temp, contents, encoding, cancellationToken), path);
    }

    /// <summary>
    /// Asynchronously opens a file or creates a file if it does not already exist, appends
    /// the specified string to the file, and then closes the file.
    /// </summary>
    /// <param name="path">
    /// The file to append the specified string to.
    /// </param>
    /// <param name="contents">
    /// The string to append to the file.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="System.Threading.CancellationToken.None" />.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous append operation.
    /// </returns>
    /// <remarks>
    /// Uses temp file.
    /// </remarks>
    public static Task AppendAllTextAsync(string path, string? contents, CancellationToken cancellationToken = default)
    {
        return WithTempFile(temp => Underlying.AppendAllTextAsync(temp, contents, cancellationToken), path);
    }

    //
    // Summary:
    //     Creates a System.IO.StreamWriter that appends UTF-8 encoded text to an existing
    //     file, or to a new file if the specified file does not exist.
    //
    // Parameters:
    //   path:
    //     The path to the file to append to.
    //
    // Returns:
    //     A stream writer that appends UTF-8 encoded text to the specified file or to a
    //     new file.
    //
    // Exceptions:
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, the directory doesn't exist or it
    //     is on an unmapped drive).
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static StreamWriter AppendText(string path)
    {
        return Underlying.AppendText(path);
    }

    /// <summary>
    /// Copies an existing file to a new file. Overwriting a file of the same name is
    /// not allowed.
    /// </summary>
    /// <param name="sourceFileName">
    /// The file to copy.
    /// </param>
    /// <param name="destFileName">
    /// The name of the destination file. This cannot be a directory or an existing file.
    /// </param>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// The caller does not have the required permission.
    /// </exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="sourceFile" /> or <paramref name="destFileName" /> is a zero-length string, contains only white space,
    /// or contains one or more invalid characters as defined by <see cref="System.IO.Path.InvalidPathChars" />.
    /// -or- <paramref name="sourceFile" /> or <paramref name="destFileName" /> specifies a directory.
    /// </exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sourceFile" /> or <paramref name="destFileName" /> is null.
    /// </exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// The specified path, file name, or both exceed the system-defined maximum length.
    /// </exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">
    /// The path specified in <paramref name="sourceFile" /> or <paramref name="destFileName" /> is invalid (for example,
    /// it is on an unmapped drive).
    /// </exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="sourceFile" /> was not found.
    /// </exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="destFileName" /> exists. -or- An I/O error has occurred.
    /// </exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="sourceFile" /> or <paramref name="destFileName" /> is in an invalid format.
    /// </exception>
    /// <remarks>
    /// Direct passthrough.
    /// </remarks>
    public static void Copy(string sourceFileName, string destFileName)
    {
        Underlying.Copy(sourceFileName, destFileName);
    }

    //
    // Summary:
    //     Copies an existing file to a new file. Overwriting a file of the same name is
    //     allowed.
    //
    // Parameters:
    //   sourceFileName:
    //     The file to copy.
    //
    //   destFileName:
    //     The name of the destination file. This cannot be a directory.
    //
    //   overwrite:
    //     true if the destination file can be overwritten; otherwise, false.
    //
    // Exceptions:
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission. -or- destFileName is read-only.
    //     -or- overwrite is true, destFileName exists and is hidden, but sourceFileName
    //     is not hidden.
    //
    //   T:System.ArgumentException:
    //     sourceFileName or destFileName is a zero-length string, contains only white space,
    //     or contains one or more invalid characters as defined by System.IO.Path.InvalidPathChars.
    //     -or- sourceFileName or destFileName specifies a directory.
    //
    //   T:System.ArgumentNullException:
    //     sourceFileName or destFileName is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The path specified in sourceFileName or destFileName is invalid (for example,
    //     it is on an unmapped drive).
    //
    //   T:System.IO.FileNotFoundException:
    //     sourceFileName was not found.
    //
    //   T:System.IO.IOException:
    //     destFileName exists and overwrite is false. -or- An I/O error has occurred.
    //
    //   T:System.NotSupportedException:
    //     sourceFileName or destFileName is in an invalid format.
    public static void Copy(string sourceFileName, string destFileName, bool overwrite)
    {
        Underlying.Copy(sourceFileName, destFileName, overwrite);
    }

    //
    // Summary:
    //     Creates or overwrites a file in the specified path.
    //
    // Parameters:
    //   path:
    //     The path and name of the file to create.
    //
    // Returns:
    //     A System.IO.FileStream that provides read/write access to the file specified
    //     in path.
    //
    // Exceptions:
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission. -or- path specified a file
    //     that is read-only. -or- path specified a file that is hidden.
    //
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while creating the file.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static FileStream Create(string path)
    {
        return Underlying.Create(path);
    }

    //
    // Summary:
    //     Creates or overwrites a file in the specified path, specifying a buffer size.
    //
    // Parameters:
    //   path:
    //     The path and name of the file to create.
    //
    //   bufferSize:
    //     The number of bytes buffered for reads and writes to the file.
    //
    // Returns:
    //     A System.IO.FileStream with the specified buffer size that provides read/write
    //     access to the file specified in path.
    //
    // Exceptions:
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission. -or- path specified a file
    //     that is read-only. -or- path specified a file that is hidden.
    //
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while creating the file.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static FileStream Create(string path, int bufferSize)
    {
        return Underlying.Create(path, bufferSize);
    }

    //
    // Summary:
    //     Creates or overwrites a file in the specified path, specifying a buffer size
    //     and options that describe how to create or overwrite the file.
    //
    // Parameters:
    //   path:
    //     The path and name of the file to create.
    //
    //   bufferSize:
    //     The number of bytes buffered for reads and writes to the file.
    //
    //   options:
    //     One of the System.IO.FileOptions values that describes how to create or overwrite
    //     the file.
    //
    // Returns:
    //     A new file with the specified buffer size.
    //
    // Exceptions:
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission. -or- path specified a file
    //     that is read-only. -or- path specified a file that is hidden.
    //
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, it is on an unmapped drive.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while creating the file.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static FileStream Create(string path, int bufferSize, FileOptions options)
    {
        return Underlying.Create(path, bufferSize, options);
    }

    //
    // Summary:
    //     Creates or opens a file for writing UTF-8 encoded text. If the file already exists,
    //     its contents are overwritten.
    //
    // Parameters:
    //   path:
    //     The file to be opened for writing.
    //
    // Returns:
    //     A System.IO.StreamWriter that writes to the specified file using UTF-8 encoding.
    //
    // Exceptions:
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission. -or- path specified a file
    //     that is read-only. -or- path specified a file that is hidden.
    //
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static StreamWriter CreateText(string path)
    {
        return Underlying.CreateText(path);
    }

    //
    // Summary:
    //     Decrypts a file that was encrypted by the current account using the System.IO.File.Encrypt(System.String)
    //     method.
    //
    // Parameters:
    //   path:
    //     A path that describes a file to decrypt.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     The path parameter is a zero-length string, contains only white space, or contains
    //     one or more invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     The path parameter is null.
    //
    //   T:System.IO.DriveNotFoundException:
    //     An invalid drive was specified.
    //
    //   T:System.IO.FileNotFoundException:
    //     The file described by the path parameter could not be found.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file. For example, the encrypted file
    //     is already open. -or- This operation is not supported on the current platform.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.PlatformNotSupportedException:
    //     The current operating system is not Windows NT or later.
    //
    //   T:System.NotSupportedException:
    //     The file system is not NTFS.
    //
    //   T:System.UnauthorizedAccessException:
    //     The path parameter specified a file that is read-only. -or- This operation is
    //     not supported on the current platform. -or- The path parameter specified a directory.
    //     -or- The caller does not have the required permission.
    [SupportedOSPlatform("windows")]
    public static void Decrypt(string path)
    {
        WithTempFile(temp => Underlying.Decrypt(temp), path);
    }

    //
    // Summary:
    //     Deletes the specified file.
    //
    // Parameters:
    //   path:
    //     The name of the file to be deleted. Wildcard characters are not supported.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     The specified file is in use. -or- There is an open handle on the file, and the
    //     operating system is Windows XP or earlier. This open handle can result from enumerating
    //     directories and files. For more information, see How to: Enumerate Directories
    //     and Files.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission. -or- The file is an executable
    //     file that is in use. -or- path is a directory. -or- path specified a read-only
    //     file.
    public static void Delete(string path)
    {
        Underlying.Delete(path);
    }

    //
    // Summary:
    //     Encrypts a file so that only the account used to encrypt the file can decrypt
    //     it.
    //
    // Parameters:
    //   path:
    //     A path that describes a file to encrypt.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     The path parameter is a zero-length string, contains only white space, or contains
    //     one or more invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     The path parameter is null.
    //
    //   T:System.IO.DriveNotFoundException:
    //     An invalid drive was specified.
    //
    //   T:System.IO.FileNotFoundException:
    //     The file described by the path parameter could not be found.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file. -or- This operation is not supported
    //     on the current platform.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.PlatformNotSupportedException:
    //     The current operating system is not Windows NT or later.
    //
    //   T:System.NotSupportedException:
    //     The file system is not NTFS.
    //
    //   T:System.UnauthorizedAccessException:
    //     The path parameter specified a file that is read-only. -or- This operation is
    //     not supported on the current platform. -or- The path parameter specified a directory.
    //     -or- The caller does not have the required permission.
    [SupportedOSPlatform("windows")]
    public static void Encrypt(string path)
    {
        WithTempFile(temp => Underlying.Encrypt(temp), path);
    }

    /// <summary>
    /// Determines whether the specified file exists.
    /// </summary>
    /// <param name="path">
    /// The file to check.
    /// </param>
    /// <returns>
    /// true if the caller has the required permissions and <paramref name="path" /> contains the name of
    /// an existing file; otherwise, false. This method also returns false if <paramref name="path" /> is
    /// null, an invalid path, or a zero-length string. If the caller does not have sufficient
    /// permissions to read the specified file, no exception is thrown and the method
    /// returns false regardless of the existence of <paramref name="path" />.
    /// </returns>
    /// <remarks>
    /// Direct passthrough.
    /// </remarks>
    public static bool Exists(string? path)
    {
        return Underlying.Exists(path);
    }

    //
    // Summary:
    //     Gets the System.IO.FileAttributes of the file on the path.
    //
    // Parameters:
    //   path:
    //     The path to the file.
    //
    // Returns:
    //     The System.IO.FileAttributes of the file on the path.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is empty, contains only white spaces, or contains invalid characters.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.IO.FileNotFoundException:
    //     path represents a file and is invalid, such as being on an unmapped drive, or
    //     the file cannot be found.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     path represents a directory and is invalid, such as being on an unmapped drive,
    //     or the directory cannot be found.
    //
    //   T:System.IO.IOException:
    //     This file is being used by another process.
    //
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    public static FileAttributes GetAttributes(string path)
    {
        return Underlying.GetAttributes(path);
    }

    //
    // Summary:
    //     Returns the creation date and time of the specified file or directory.
    //
    // Parameters:
    //   path:
    //     The file or directory for which to obtain creation date and time information.
    //
    // Returns:
    //     A System.DateTime structure set to the creation date and time for the specified
    //     file or directory. This value is expressed in local time.
    //
    // Exceptions:
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static DateTime GetCreationTime(string path)
    {
        return Underlying.GetCreationTime(path);
    }

    //
    // Summary:
    //     Returns the creation date and time, in coordinated universal time (UTC), of the
    //     specified file or directory.
    //
    // Parameters:
    //   path:
    //     The file or directory for which to obtain creation date and time information.
    //
    // Returns:
    //     A System.DateTime structure set to the creation date and time for the specified
    //     file or directory. This value is expressed in UTC time.
    //
    // Exceptions:
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static DateTime GetCreationTimeUtc(string path)
    {
        return Underlying.GetCreationTimeUtc(path);
    }

    //
    // Summary:
    //     Returns the date and time the specified file or directory was last accessed.
    //
    // Parameters:
    //   path:
    //     The file or directory for which to obtain access date and time information.
    //
    // Returns:
    //     A System.DateTime structure set to the date and time that the specified file
    //     or directory was last accessed. This value is expressed in local time.
    //
    // Exceptions:
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static DateTime GetLastAccessTime(string path)
    {
        return Underlying.GetLastAccessTime(path);
    }

    //
    // Summary:
    //     Returns the date and time, in coordinated universal time (UTC), that the specified
    //     file or directory was last accessed.
    //
    // Parameters:
    //   path:
    //     The file or directory for which to obtain access date and time information.
    //
    // Returns:
    //     A System.DateTime structure set to the date and time that the specified file
    //     or directory was last accessed. This value is expressed in UTC time.
    //
    // Exceptions:
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static DateTime GetLastAccessTimeUtc(string path)
    {
        return Underlying.GetLastAccessTimeUtc(path);
    }

    //
    // Summary:
    //     Returns the date and time the specified file or directory was last written to.
    //
    // Parameters:
    //   path:
    //     The file or directory for which to obtain write date and time information.
    //
    // Returns:
    //     A System.DateTime structure set to the date and time that the specified file
    //     or directory was last written to. This value is expressed in local time.
    //
    // Exceptions:
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static DateTime GetLastWriteTime(string path)
    {
        return Underlying.GetLastWriteTime(path);
    }

    //
    // Summary:
    //     Returns the date and time, in coordinated universal time (UTC), that the specified
    //     file or directory was last written to.
    //
    // Parameters:
    //   path:
    //     The file or directory for which to obtain write date and time information.
    //
    // Returns:
    //     A System.DateTime structure set to the date and time that the specified file
    //     or directory was last written to. This value is expressed in UTC time.
    //
    // Exceptions:
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static DateTime GetLastWriteTimeUtc(string path)
    {
        return Underlying.GetLastWriteTimeUtc(path);
    }

    //
    // Summary:
    //     Moves a specified file to a new location, providing the option to specify a new
    //     file name.
    //
    // Parameters:
    //   sourceFileName:
    //     The name of the file to move. Can include a relative or absolute path.
    //
    //   destFileName:
    //     The new path and name for the file.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     destFileName already exists.
    //
    //   T:System.IO.FileNotFoundException:
    //     sourceFileName was not found.
    //
    //   T:System.ArgumentNullException:
    //     sourceFileName or destFileName is null.
    //
    //   T:System.ArgumentException:
    //     sourceFileName or destFileName is a zero-length string, contains only white space,
    //     or contains invalid characters as defined in System.IO.Path.InvalidPathChars.
    //
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The path specified in sourceFileName or destFileName is invalid, (for example,
    //     it is on an unmapped drive).
    //
    //   T:System.NotSupportedException:
    //     sourceFileName or destFileName is in an invalid format.
    public static void Move(string sourceFileName, string destFileName)
    {
        Underlying.Move(sourceFileName, destFileName);
    }

    //
    // Summary:
    //     Moves a specified file to a new location, providing the options to specify a
    //     new file name and to overwrite the destination file if it already exists.
    //
    // Parameters:
    //   sourceFileName:
    //     The name of the file to move. Can include a relative or absolute path.
    //
    //   destFileName:
    //     The new path and name for the file.
    //
    //   overwrite:
    //     true to overwrite the destination file if it already exists; false otherwise.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     destFileName already exists and overwrite is false.
    //
    //   T:System.IO.FileNotFoundException:
    //     sourceFileName was not found.
    //
    //   T:System.ArgumentNullException:
    //     sourceFileName or destFileName is null.
    //
    //   T:System.ArgumentException:
    //     sourceFileName or destFileName is a zero-length string, contains only white space,
    //     or contains invalid characters as defined in System.IO.Path.InvalidPathChars.
    //
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The path specified in sourceFileName or destFileName is invalid, (for example,
    //     it is on an unmapped drive).
    //
    //   T:System.NotSupportedException:
    //     sourceFileName or destFileName is in an invalid format.
    public static void Move(string sourceFileName, string destFileName, bool overwrite)
    {
        Underlying.Move(sourceFileName, destFileName, overwrite);
    }

    //
    // Summary:
    //     Opens a System.IO.FileStream on the specified path with read/write access with
    //     no sharing.
    //
    // Parameters:
    //   path:
    //     The file to open.
    //
    //   mode:
    //     A System.IO.FileMode value that specifies whether a file is created if one does
    //     not exist, and determines whether the contents of existing files are retained
    //     or overwritten.
    //
    // Returns:
    //     A System.IO.FileStream opened in the specified mode and path, with read/write
    //     access and not shared.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid, (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only. -or- This operation is not supported
    //     on the current platform. -or- path specified a directory. -or- The caller does
    //     not have the required permission. -or- mode is System.IO.FileMode.Create and
    //     the specified file is a hidden file.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     mode specified an invalid value.
    //
    //   T:System.IO.FileNotFoundException:
    //     The file specified in path was not found.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static FileStream Open(string path, FileMode mode)
    {
        return Underlying.Open(path, mode);
    }

    //
    // Summary:
    //     Opens a System.IO.FileStream on the specified path, with the specified mode and
    //     access with no sharing.
    //
    // Parameters:
    //   path:
    //     The file to open.
    //
    //   mode:
    //     A System.IO.FileMode value that specifies whether a file is created if one does
    //     not exist, and determines whether the contents of existing files are retained
    //     or overwritten.
    //
    //   access:
    //     A System.IO.FileAccess value that specifies the operations that can be performed
    //     on the file.
    //
    // Returns:
    //     An unshared System.IO.FileStream that provides access to the specified file,
    //     with the specified mode and access.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars. -or- access
    //     specified Read and mode specified Create, CreateNew, Truncate, or Append.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid, (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only and access is not Read. -or- path specified
    //     a directory. -or- The caller does not have the required permission. -or- mode
    //     is System.IO.FileMode.Create and the specified file is a hidden file.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     mode or access specified an invalid value.
    //
    //   T:System.IO.FileNotFoundException:
    //     The file specified in path was not found.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static FileStream Open(string path, FileMode mode, FileAccess access)
    {
        return Underlying.Open(path, mode, access);
    }

    //
    // Summary:
    //     Opens a System.IO.FileStream on the specified path, having the specified mode
    //     with read, write, or read/write access and the specified sharing option.
    //
    // Parameters:
    //   path:
    //     The file to open.
    //
    //   mode:
    //     A System.IO.FileMode value that specifies whether a file is created if one does
    //     not exist, and determines whether the contents of existing files are retained
    //     or overwritten.
    //
    //   access:
    //     A System.IO.FileAccess value that specifies the operations that can be performed
    //     on the file.
    //
    //   share:
    //     A System.IO.FileShare value specifying the type of access other threads have
    //     to the file.
    //
    // Returns:
    //     A System.IO.FileStream on the specified path, having the specified mode with
    //     read, write, or read/write access and the specified sharing option.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars. -or- access
    //     specified Read and mode specified Create, CreateNew, Truncate, or Append.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid, (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only and access is not Read. -or- path specified
    //     a directory. -or- The caller does not have the required permission. -or- mode
    //     is System.IO.FileMode.Create and the specified file is a hidden file.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     mode, access, or share specified an invalid value.
    //
    //   T:System.IO.FileNotFoundException:
    //     The file specified in path was not found.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
    {
        return Underlying.Open(path, mode, access, share);
    }

    //
    // Summary:
    //     Opens an existing file for reading.
    //
    // Parameters:
    //   path:
    //     The file to be opened for reading.
    //
    // Returns:
    //     A read-only System.IO.FileStream on the specified path.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid, (for example, it is on an unmapped drive).
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a directory. -or- The caller does not have the required permission.
    //
    //   T:System.IO.FileNotFoundException:
    //     The file specified in path was not found.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    public static FileStream OpenRead(string path)
    {
        return Underlying.OpenRead(path);
    }

    //
    // Summary:
    //     Opens an existing UTF-8 encoded text file for reading.
    //
    // Parameters:
    //   path:
    //     The file to be opened for reading.
    //
    // Returns:
    //     A System.IO.StreamReader on the specified path.
    //
    // Exceptions:
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid, (for example, it is on an unmapped drive).
    //
    //   T:System.IO.FileNotFoundException:
    //     The file specified in path was not found.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static StreamReader OpenText(string path)
    {
        return Underlying.OpenText(path);
    }

    //
    // Summary:
    //     Opens an existing file or creates a new file for writing.
    //
    // Parameters:
    //   path:
    //     The file to be opened for writing.
    //
    // Returns:
    //     An unshared System.IO.FileStream object on the specified path with System.IO.FileAccess.Write
    //     access.
    //
    // Exceptions:
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission. -or- path specified a read-only
    //     file or directory.
    //
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid, (for example, it is on an unmapped drive).
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static FileStream OpenWrite(string path)
    {
        return Underlying.OpenWrite(path);
    }

    //
    // Summary:
    //     Opens a binary file, reads the contents of the file into a byte array, and then
    //     closes the file.
    //
    // Parameters:
    //   path:
    //     The file to open for reading.
    //
    // Returns:
    //     A byte array containing the contents of the file.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.UnauthorizedAccessException:
    //     This operation is not supported on the current platform. -or- path specified
    //     a directory. -or- The caller does not have the required permission.
    //
    //   T:System.IO.FileNotFoundException:
    //     The file specified in path was not found.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    public static byte[] ReadAllBytes(string path)
    {
        return Underlying.ReadAllBytes(path);
    }

    //
    // Summary:
    //     Asynchronously opens a binary file, reads the contents of the file into a byte
    //     array, and then closes the file.
    //
    // Parameters:
    //   path:
    //     The file to open for reading.
    //
    //   cancellationToken:
    //     The token to monitor for cancellation requests. The default value is System.Threading.CancellationToken.None.
    //
    // Returns:
    //     A task that represents the asynchronous read operation, which wraps the byte
    //     array containing the contents of the file.
    public static Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default)
    {
        return Underlying.ReadAllBytesAsync(path, cancellationToken);
    }

    //
    // Summary:
    //     Opens a text file, reads all lines of the file, and then closes the file.
    //
    // Parameters:
    //   path:
    //     The file to open for reading.
    //
    // Returns:
    //     A string array containing all lines of the file.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only. -or- This operation is not supported
    //     on the current platform. -or- path specified a directory. -or- The caller does
    //     not have the required permission.
    //
    //   T:System.IO.FileNotFoundException:
    //     The file specified in path was not found.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    public static string[] ReadAllLines(string path)
    {
        return Underlying.ReadAllLines(path);
    }

    //
    // Summary:
    //     Opens a file, reads all lines of the file with the specified encoding, and then
    //     closes the file.
    //
    // Parameters:
    //   path:
    //     The file to open for reading.
    //
    //   encoding:
    //     The encoding applied to the contents of the file.
    //
    // Returns:
    //     A string array containing all lines of the file.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only. -or- This operation is not supported
    //     on the current platform. -or- path specified a directory. -or- The caller does
    //     not have the required permission.
    //
    //   T:System.IO.FileNotFoundException:
    //     The file specified in path was not found.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    public static string[] ReadAllLines(string path, Encoding encoding)
    {
        return Underlying.ReadAllLines(path, encoding);
    }

    //
    // Summary:
    //     Asynchronously opens a text file, reads all lines of the file with the specified
    //     encoding, and then closes the file.
    //
    // Parameters:
    //   path:
    //     The file to open for reading.
    //
    //   encoding:
    //     The encoding applied to the contents of the file.
    //
    //   cancellationToken:
    //     The token to monitor for cancellation requests. The default value is System.Threading.CancellationToken.None.
    //
    // Returns:
    //     A task that represents the asynchronous read operation, which wraps the string
    //     array containing all lines of the file.
    public static Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
    {
        return Underlying.ReadAllLinesAsync(path, encoding, cancellationToken);
    }

    //
    // Summary:
    //     Asynchronously opens a text file, reads all lines of the file, and then closes
    //     the file.
    //
    // Parameters:
    //   path:
    //     The file to open for reading.
    //
    //   cancellationToken:
    //     The token to monitor for cancellation requests. The default value is System.Threading.CancellationToken.None.
    //
    // Returns:
    //     A task that represents the asynchronous read operation, which wraps the string
    //     array containing all lines of the file.
    public static Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default)
    {
        return Underlying.ReadAllLinesAsync(path, cancellationToken);
    }

    //
    // Summary:
    //     Opens a text file, reads all the text in the file, and then closes the file.
    //
    // Parameters:
    //   path:
    //     The file to open for reading.
    //
    // Returns:
    //     A string containing all the text in the file.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only. -or- This operation is not supported
    //     on the current platform. -or- path specified a directory. -or- The caller does
    //     not have the required permission.
    //
    //   T:System.IO.FileNotFoundException:
    //     The file specified in path was not found.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    public static string ReadAllText(string path)
    {
        return Underlying.ReadAllText(path);
    }

    //
    // Summary:
    //     Opens a file, reads all text in the file with the specified encoding, and then
    //     closes the file.
    //
    // Parameters:
    //   path:
    //     The file to open for reading.
    //
    //   encoding:
    //     The encoding applied to the contents of the file.
    //
    // Returns:
    //     A string containing all text in the file.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only. -or- This operation is not supported
    //     on the current platform. -or- path specified a directory. -or- The caller does
    //     not have the required permission.
    //
    //   T:System.IO.FileNotFoundException:
    //     The file specified in path was not found.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    public static string ReadAllText(string path, Encoding encoding)
    {
        return Underlying.ReadAllText(path, encoding);
    }

    //
    // Summary:
    //     Asynchronously opens a text file, reads all text in the file with the specified
    //     encoding, and then closes the file.
    //
    // Parameters:
    //   path:
    //     The file to open for reading.
    //
    //   encoding:
    //     The encoding applied to the contents of the file.
    //
    //   cancellationToken:
    //     The token to monitor for cancellation requests. The default value is System.Threading.CancellationToken.None.
    //
    // Returns:
    //     A task that represents the asynchronous read operation, which wraps the string
    //     containing all text in the file.
    public static Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
    {
        return Underlying.ReadAllTextAsync(path, encoding, cancellationToken);
    }

    //
    // Summary:
    //     Asynchronously opens a text file, reads all the text in the file, and then closes
    //     the file.
    //
    // Parameters:
    //   path:
    //     The file to open for reading.
    //
    //   cancellationToken:
    //     The token to monitor for cancellation requests. The default value is System.Threading.CancellationToken.None.
    //
    // Returns:
    //     A task that represents the asynchronous read operation, which wraps the string
    //     containing all text in the file.
    public static Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
    {
        return Underlying.ReadAllTextAsync(path, cancellationToken);
    }

    //
    // Summary:
    //     Reads the lines of a file.
    //
    // Parameters:
    //   path:
    //     The file to read.
    //
    // Returns:
    //     All the lines of the file, or the lines that are the result of a query.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters defined by the System.IO.Path.GetInvalidPathChars method.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.FileNotFoundException:
    //     The file specified by path was not found.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.IO.PathTooLongException:
    //     path exceeds the system-defined maximum length.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specifies a file that is read-only. -or- This operation is not supported
    //     on the current platform. -or- path is a directory. -or- The caller does not have
    //     the required permission.
    public static IEnumerable<string> ReadLines(string path)
    {
        return Underlying.ReadLines(path);
    }

    //
    // Summary:
    //     Read the lines of a file that has a specified encoding.
    //
    // Parameters:
    //   path:
    //     The file to read.
    //
    //   encoding:
    //     The encoding that is applied to the contents of the file.
    //
    // Returns:
    //     All the lines of the file, or the lines that are the result of a query.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by the System.IO.Path.GetInvalidPathChars method.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.FileNotFoundException:
    //     The file specified by path was not found.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.IO.PathTooLongException:
    //     path exceeds the system-defined maximum length.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specifies a file that is read-only. -or- This operation is not supported
    //     on the current platform. -or- path is a directory. -or- The caller does not have
    //     the required permission.
    public static IEnumerable<string> ReadLines(string path, Encoding encoding)
    {
        return Underlying.ReadLines(path, encoding);
    }

    //
    // Summary:
    //     Replaces the contents of a specified file with the contents of another file,
    //     deleting the original file, and creating a backup of the replaced file.
    //
    // Parameters:
    //   sourceFileName:
    //     The name of a file that replaces the file specified by destinationFileName.
    //
    //   destinationFileName:
    //     The name of the file being replaced.
    //
    //   destinationBackupFileName:
    //     The name of the backup file.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     The path described by the destinationFileName parameter was not of a legal form.
    //     -or- The path described by the destinationBackupFileName parameter was not of
    //     a legal form.
    //
    //   T:System.ArgumentNullException:
    //     The destinationFileName parameter is null.
    //
    //   T:System.IO.DriveNotFoundException:
    //     An invalid drive was specified.
    //
    //   T:System.IO.FileNotFoundException:
    //     The file described by the current System.IO.FileInfo object could not be found.
    //     -or- The file described by the destinationBackupFileName parameter could not
    //     be found.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file. -or- The sourceFileName and destinationFileName
    //     parameters specify the same file.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.PlatformNotSupportedException:
    //     The operating system is Windows 98 Second Edition or earlier and the files system
    //     is not NTFS.
    //
    //   T:System.UnauthorizedAccessException:
    //     The sourceFileName or destinationFileName parameter specifies a file that is
    //     read-only. -or- This operation is not supported on the current platform. -or-
    //     Source or destination parameters specify a directory instead of a file. -or-
    //     The caller does not have the required permission.
    public static void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName)
    {
        Underlying.Replace(sourceFileName, destinationFileName, destinationBackupFileName);
    }

    //
    // Summary:
    //     Replaces the contents of a specified file with the contents of another file,
    //     deleting the original file, and creating a backup of the replaced file and optionally
    //     ignores merge errors.
    //
    // Parameters:
    //   sourceFileName:
    //     The name of a file that replaces the file specified by destinationFileName.
    //
    //   destinationFileName:
    //     The name of the file being replaced.
    //
    //   destinationBackupFileName:
    //     The name of the backup file.
    //
    //   ignoreMetadataErrors:
    //     true to ignore merge errors (such as attributes and access control lists (ACLs))
    //     from the replaced file to the replacement file; otherwise, false.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     The path described by the destinationFileName parameter was not of a legal form.
    //     -or- The path described by the destinationBackupFileName parameter was not of
    //     a legal form.
    //
    //   T:System.ArgumentNullException:
    //     The destinationFileName parameter is null.
    //
    //   T:System.IO.DriveNotFoundException:
    //     An invalid drive was specified.
    //
    //   T:System.IO.FileNotFoundException:
    //     The file described by the current System.IO.FileInfo object could not be found.
    //     -or- The file described by the destinationBackupFileName parameter could not
    //     be found.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file. -or- The sourceFileName and destinationFileName
    //     parameters specify the same file.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.PlatformNotSupportedException:
    //     The operating system is Windows 98 Second Edition or earlier and the files system
    //     is not NTFS.
    //
    //   T:System.UnauthorizedAccessException:
    //     The sourceFileName or destinationFileName parameter specifies a file that is
    //     read-only. -or- This operation is not supported on the current platform. -or-
    //     Source or destination parameters specify a directory instead of a file. -or-
    //     The caller does not have the required permission.
    public static void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName, bool ignoreMetadataErrors)
    {
        Underlying.Replace(sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
    }

    //
    // Summary:
    //     Sets the specified System.IO.FileAttributes of the file on the specified path.
    //
    // Parameters:
    //   path:
    //     The path to the file.
    //
    //   fileAttributes:
    //     A bitwise combination of the enumeration values.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is empty, contains only white spaces, contains invalid characters, or the
    //     file attribute is invalid.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid, (for example, it is on an unmapped drive).
    //
    //   T:System.IO.FileNotFoundException:
    //     The file cannot be found.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only. -or- This operation is not supported
    //     on the current platform. -or- path specified a directory. -or- The caller does
    //     not have the required permission.
    public static void SetAttributes(string path, FileAttributes fileAttributes)
    {
        Underlying.SetAttributes(path, fileAttributes);
    }

    //
    // Summary:
    //     Sets the date and time the file was created.
    //
    // Parameters:
    //   path:
    //     The file for which to set the creation date and time information.
    //
    //   creationTime:
    //     A System.DateTime containing the value to set for the creation date and time
    //     of path. This value is expressed in local time.
    //
    // Exceptions:
    //   T:System.IO.FileNotFoundException:
    //     The specified path was not found.
    //
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while performing the operation.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     creationTime specifies a value outside the range of dates, times, or both permitted
    //     for this operation.
    //
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static void SetCreationTime(string path, DateTime creationTime)
    {
        Underlying.SetCreationTime(path, creationTime);
    }

    //
    // Summary:
    //     Sets the date and time, in coordinated universal time (UTC), that the file was
    //     created.
    //
    // Parameters:
    //   path:
    //     The file for which to set the creation date and time information.
    //
    //   creationTimeUtc:
    //     A System.DateTime containing the value to set for the creation date and time
    //     of path. This value is expressed in UTC time.
    //
    // Exceptions:
    //   T:System.IO.FileNotFoundException:
    //     The specified path was not found.
    //
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while performing the operation.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     creationTime specifies a value outside the range of dates, times, or both permitted
    //     for this operation.
    //
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
    {
        Underlying.SetCreationTimeUtc(path, creationTimeUtc);
    }

    //
    // Summary:
    //     Sets the date and time the specified file was last accessed.
    //
    // Parameters:
    //   path:
    //     The file for which to set the access date and time information.
    //
    //   lastAccessTime:
    //     A System.DateTime containing the value to set for the last access date and time
    //     of path. This value is expressed in local time.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.FileNotFoundException:
    //     The specified path was not found.
    //
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     lastAccessTime specifies a value outside the range of dates or times permitted
    //     for this operation.
    public static void SetLastAccessTime(string path, DateTime lastAccessTime)
    {
        Underlying.SetLastAccessTime(path, lastAccessTime);
    }

    //
    // Summary:
    //     Sets the date and time, in coordinated universal time (UTC), that the specified
    //     file was last accessed.
    //
    // Parameters:
    //   path:
    //     The file for which to set the access date and time information.
    //
    //   lastAccessTimeUtc:
    //     A System.DateTime containing the value to set for the last access date and time
    //     of path. This value is expressed in UTC time.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.FileNotFoundException:
    //     The specified path was not found.
    //
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     lastAccessTimeUtc specifies a value outside the range of dates or times permitted
    //     for this operation.
    public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
    {
        Underlying.SetLastAccessTimeUtc(path, lastAccessTimeUtc);
    }

    //
    // Summary:
    //     Sets the date and time that the specified file was last written to.
    //
    // Parameters:
    //   path:
    //     The file for which to set the date and time information.
    //
    //   lastWriteTime:
    //     A System.DateTime containing the value to set for the last write date and time
    //     of path. This value is expressed in local time.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.FileNotFoundException:
    //     The specified path was not found.
    //
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     lastWriteTime specifies a value outside the range of dates or times permitted
    //     for this operation.
    public static void SetLastWriteTime(string path, DateTime lastWriteTime)
    {
        Underlying.SetLastWriteTime(path, lastWriteTime);
    }

    //
    // Summary:
    //     Sets the date and time, in coordinated universal time (UTC), that the specified
    //     file was last written to.
    //
    // Parameters:
    //   path:
    //     The file for which to set the date and time information.
    //
    //   lastWriteTimeUtc:
    //     A System.DateTime containing the value to set for the last write date and time
    //     of path. This value is expressed in UTC time.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.FileNotFoundException:
    //     The specified path was not found.
    //
    //   T:System.UnauthorizedAccessException:
    //     The caller does not have the required permission.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     lastWriteTimeUtc specifies a value outside the range of dates or times permitted
    //     for this operation.
    public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
    {
        Underlying.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
    }

    //
    // Summary:
    //     Creates a new file, writes the specified byte array to the file, and then closes
    //     the file. If the target file already exists, it is overwritten.
    //
    // Parameters:
    //   path:
    //     The file to write to.
    //
    //   bytes:
    //     The bytes to write to the file.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null or the byte array is empty.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only. -or- path specified a file that is hidden.
    //     -or- This operation is not supported on the current platform. -or- path specified
    //     a directory. -or- The caller does not have the required permission.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    public static void WriteAllBytes(string path, byte[] bytes)
    {
        WithTempFile(temp => Underlying.WriteAllBytes(temp, bytes), path);
    }

    //
    // Summary:
    //     Asynchronously creates a new file, writes the specified byte array to the file,
    //     and then closes the file. If the target file already exists, it is overwritten.
    //
    // Parameters:
    //   path:
    //     The file to write to.
    //
    //   bytes:
    //     The bytes to write to the file.
    //
    //   cancellationToken:
    //     The token to monitor for cancellation requests. The default value is System.Threading.CancellationToken.None.
    //
    // Returns:
    //     A task that represents the asynchronous write operation.
    public static Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default)
    {
        return WithTempFile(temp => Underlying.WriteAllBytesAsync(temp, bytes, cancellationToken), path);
    }

    //
    // Summary:
    //     Creates a new file, writes a collection of strings to the file, and then closes
    //     the file.
    //
    // Parameters:
    //   path:
    //     The file to write to.
    //
    //   contents:
    //     The lines to write to the file.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters defined by the System.IO.Path.GetInvalidPathChars method.
    //
    //   T:System.ArgumentNullException:
    //     Either path or contents is null.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.IO.PathTooLongException:
    //     path exceeds the system-defined maximum length.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only. -or- path specified a file that is hidden.
    //     -or- This operation is not supported on the current platform. -or- path is a
    //     directory. -or- The caller does not have the required permission.
    public static void WriteAllLines(string path, IEnumerable<string> contents)
    {
        WithTempFile(temp => Underlying.WriteAllLines(temp, contents), path);
    }

    //
    // Summary:
    //     Creates a new file by using the specified encoding, writes a collection of strings
    //     to the file, and then closes the file.
    //
    // Parameters:
    //   path:
    //     The file to write to.
    //
    //   contents:
    //     The lines to write to the file.
    //
    //   encoding:
    //     The character encoding to use.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters defined by the System.IO.Path.GetInvalidPathChars method.
    //
    //   T:System.ArgumentNullException:
    //     Either path, contents, or encoding is null.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.IO.PathTooLongException:
    //     path exceeds the system-defined maximum length.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only. -or- path specified a file that is hidden.
    //     -or- This operation is not supported on the current platform. -or- path is a
    //     directory. -or- The caller does not have the required permission.
    public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
    {
        WithTempFile(temp => Underlying.WriteAllLines(temp, contents, encoding), path);
    }

    //
    // Summary:
    //     Creates a new file, write the specified string array to the file, and then closes
    //     the file.
    //
    // Parameters:
    //   path:
    //     The file to write to.
    //
    //   contents:
    //     The string array to write to the file.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     Either path or contents is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only. -or- path specified a file that is hidden.
    //     -or- This operation is not supported on the current platform. -or- path specified
    //     a directory. -or- The caller does not have the required permission.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    public static void WriteAllLines(string path, string[] contents)
    {
        WithTempFile(temp => Underlying.WriteAllLines(temp, contents), path);
    }

    //
    // Summary:
    //     Creates a new file, writes the specified string array to the file by using the
    //     specified encoding, and then closes the file.
    //
    // Parameters:
    //   path:
    //     The file to write to.
    //
    //   contents:
    //     The string array to write to the file.
    //
    //   encoding:
    //     An System.Text.Encoding object that represents the character encoding applied
    //     to the string array.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     Either path or contents is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only. -or- path specified a file that is hidden.
    //     -or- This operation is not supported on the current platform. -or- path specified
    //     a directory. -or- The caller does not have the required permission.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    public static void WriteAllLines(string path, string[] contents, Encoding encoding)
    {
        WithTempFile(temp => Underlying.WriteAllLines(temp, contents, encoding), path);
    }

    //
    // Summary:
    //     Asynchronously creates a new file, write the specified lines to the file by using
    //     the specified encoding, and then closes the file.
    //
    // Parameters:
    //   path:
    //     The file to write to.
    //
    //   contents:
    //     The lines to write to the file.
    //
    //   encoding:
    //     The character encoding to use.
    //
    //   cancellationToken:
    //     The token to monitor for cancellation requests. The default value is System.Threading.CancellationToken.None.
    //
    // Returns:
    //     A task that represents the asynchronous write operation.
    public static Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        return WithTempFile(temp => Underlying.WriteAllLinesAsync(temp, contents, encoding, cancellationToken), path);
    }

    //
    // Summary:
    //     Asynchronously creates a new file, writes the specified lines to the file, and
    //     then closes the file.
    //
    // Parameters:
    //   path:
    //     The file to write to.
    //
    //   contents:
    //     The lines to write to the file.
    //
    //   cancellationToken:
    //     The token to monitor for cancellation requests. The default value is System.Threading.CancellationToken.None.
    //
    // Returns:
    //     A task that represents the asynchronous write operation.
    public static Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
    {
        return WithTempFile(temp => Underlying.WriteAllLinesAsync(temp, contents, cancellationToken), path);
    }

    //
    // Summary:
    //     Creates a new file, writes the specified string to the file, and then closes
    //     the file. If the target file already exists, it is overwritten.
    //
    // Parameters:
    //   path:
    //     The file to write to.
    //
    //   contents:
    //     The string to write to the file.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only. -or- path specified a file that is hidden.
    //     -or- This operation is not supported on the current platform. -or- path specified
    //     a directory. -or- The caller does not have the required permission.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    public static void WriteAllText(string path, string? contents)
    {
        WithTempFile(temp => Underlying.WriteAllText(temp, contents), path);
    }

    //
    // Summary:
    //     Creates a new file, writes the specified string to the file using the specified
    //     encoding, and then closes the file. If the target file already exists, it is
    //     overwritten.
    //
    // Parameters:
    //   path:
    //     The file to write to.
    //
    //   contents:
    //     The string to write to the file.
    //
    //   encoding:
    //     The encoding to apply to the string.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     path is a zero-length string, contains only white space, or contains one or more
    //     invalid characters as defined by System.IO.Path.InvalidPathChars.
    //
    //   T:System.ArgumentNullException:
    //     path is null.
    //
    //   T:System.IO.PathTooLongException:
    //     The specified path, file name, or both exceed the system-defined maximum length.
    //
    //   T:System.IO.DirectoryNotFoundException:
    //     The specified path is invalid (for example, it is on an unmapped drive).
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred while opening the file.
    //
    //   T:System.UnauthorizedAccessException:
    //     path specified a file that is read-only. -or- path specified a file that is hidden.
    //     -or- This operation is not supported on the current platform. -or- path specified
    //     a directory. -or- The caller does not have the required permission.
    //
    //   T:System.NotSupportedException:
    //     path is in an invalid format.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    public static void WriteAllText(string path, string? contents, Encoding encoding)
    {
        WithTempFile(temp => Underlying.WriteAllText(temp, contents, encoding), path);
    }

    //
    // Summary:
    //     Asynchronously creates a new file, writes the specified string to the file using
    //     the specified encoding, and then closes the file. If the target file already
    //     exists, it is overwritten.
    //
    // Parameters:
    //   path:
    //     The file to write to.
    //
    //   contents:
    //     The string to write to the file.
    //
    //   encoding:
    //     The encoding to apply to the string.
    //
    //   cancellationToken:
    //     The token to monitor for cancellation requests. The default value is System.Threading.CancellationToken.None.
    //
    // Returns:
    //     A task that represents the asynchronous write operation.
    public static Task WriteAllTextAsync(string path, string? contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        return WithTempFile(temp => Underlying.WriteAllTextAsync(temp, contents, encoding, cancellationToken), path);
    }

    //
    // Summary:
    //     Asynchronously creates a new file, writes the specified string to the file, and
    //     then closes the file. If the target file already exists, it is overwritten.
    //
    // Parameters:
    //   path:
    //     The file to write to.
    //
    //   contents:
    //     The string to write to the file.
    //
    //   cancellationToken:
    //     The token to monitor for cancellation requests. The default value is System.Threading.CancellationToken.None.
    //
    // Returns:
    //     A task that represents the asynchronous write operation.
    public static Task WriteAllTextAsync(string path, string? contents, CancellationToken cancellationToken = default)
    {
        return WithTempFile(temp => Underlying.WriteAllTextAsync(temp, contents, cancellationToken), path);
    }

    #endregion

    #region " Private Helper Methods "

    private static void WithTempFile(Action<string> action, string path, bool overwite = true)
    {
        var tempFileName = GetTempFileName(path);
        try
        {
            action(tempFileName);
            Underlying.Move(tempFileName, path, overwite);
        }
        catch (Exception ex)
        {
            // Don't orphan any temp files.
            if (Underlying.Exists(tempFileName))
            {
                Underlying.Delete(tempFileName);
            }
            throw new Exception($"Exception encountered while working with temp file '{tempFileName}' representing '{path}'.", ex);
        }
    }

    private static T WithTempFile<T>(Func<string, T> func, string path, bool overwite = true)
    {
        var tempFileName = GetTempFileName(path);
        try
        {
            var result = func(tempFileName);
            if (result is Task task)
            {
                task.ContinueWith(_ =>  Underlying.Move(tempFileName, path, overwite));
            }
            else
            {
                Underlying.Move(tempFileName, path, overwite);
            }
            return result;
        }
        catch (Exception ex)
        {
            // Don't orphan any temp files.
            if (Underlying.Exists(tempFileName))
            {
                Underlying.Delete(tempFileName);
            }
            throw new Exception($"Exception encountered while working with temp file '{tempFileName}' representing '{path}'.", ex);
        }
    }

    private static string GetTempFileName(string path)
    {
        var tempFileName = Path.GetTempFileName();
        if (Underlying.Exists(path))
        {
            // Prepare for file modifications.
            Underlying.Copy(path, tempFileName, true); // Path.GetTempFileName creates an empty file, so overwrite it.
        }
        return tempFileName;
    }

    #endregion

}