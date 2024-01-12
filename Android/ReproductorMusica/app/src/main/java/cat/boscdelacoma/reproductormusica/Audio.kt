package cat.boscdelacoma.reproductormusica

import android.content.ContentResolver
import android.content.ContentValues
import android.content.Context
import android.database.Cursor
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.media.MediaMetadataRetriever
import android.media.MediaPlayer
import android.net.Uri
import android.os.Build
import android.os.Environment
import android.provider.MediaStore
import android.widget.ImageView
import android.widget.LinearLayout
import android.widget.SimpleCursorAdapter
import android.widget.Toast
import androidx.appcompat.app.AlertDialog
import androidx.core.net.toUri
import androidx.loader.content.CursorLoader
import java.io.BufferedReader
import java.io.File
import java.io.FileOutputStream
import java.io.InputStream
import java.io.InputStreamReader
import java.io.IOException
import java.io.OutputStream
import javax.xml.transform.URIResolver

class Audio {

    var titol: String? = ""
    var path: String? = ""
    var duration: String? = ""
    var mediaPlayer: MediaPlayer = MediaPlayer()

    constructor() {}

    public fun createFolder(folderName: String): Boolean {

        return try {
            val values = ContentValues()
            values.put(MediaStore.MediaColumns.RELATIVE_PATH, Environment.DIRECTORY_MUSIC + "/" + folderName)
            values.put(MediaStore.Images.Media.IS_PENDING, true)
            true

        } catch (e: IOException) {
            false
        }
    }

    public fun saveSong(songName: String, folderName: String, inputStream: InputStream, context: Context): Boolean {

        return try {
            val fd = context.assets.openFd(songName)

            mediaPlayer.setDataSource(
                fd.fileDescriptor,
                fd.startOffset,
                fd.length
            )

            fd.close()

            val values = ContentValues()
            values.put(MediaStore.Audio.Media.TITLE, songName)
            values.put(MediaStore.MediaColumns.DISPLAY_NAME, songName)
            values.put(MediaStore.MediaColumns.MIME_TYPE, "audio/mp3")
            values.put(MediaStore.MediaColumns.RELATIVE_PATH, Environment.DIRECTORY_MUSIC + "/" + folderName)
            values.put(MediaStore.Audio.Media.DURATION, mediaPlayer.duration)
            values.put(MediaStore.Images.Media.IS_PENDING, true)



            val saveAudio =
                context.contentResolver.insert(MediaStore.Audio.Media.EXTERNAL_CONTENT_URI, values)

            if (saveAudio != null) {
                val outputStream = saveAudio!!.let { context.contentResolver.openOutputStream(it) }
                val buffer = ByteArray(1024)
                var length: Int
                while (inputStream.read(buffer).also { length = it } > 0) {
                    outputStream?.write(buffer, 0, length)
                }
                outputStream?.flush()
                outputStream?.close()
                inputStream?.close()
            }

            Toast.makeText(
                context,
                "Audio guardat",
                Toast.LENGTH_LONG
            ).show()
            true
            
        } catch (e: IOException) {
            Toast.makeText(
                context,
                e.localizedMessage,
                Toast.LENGTH_LONG
            ).show()
            false
        }
    }
}