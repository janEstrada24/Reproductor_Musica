package cat.boscdelacoma.reproductormusica.Adapters

import android.content.Intent
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import cat.boscdelacoma.reproductormusica.Audio
import cat.boscdelacoma.reproductormusica.MainActivity
import cat.boscdelacoma.reproductormusica.R
import cat.boscdelacoma.reproductormusica.TrackSongs

class SongInTrackAdapter(private val songList: List<SongItem>): RecyclerView.Adapter<SongInTrackAdapter.ViewHolder>(){

    data class SongItem(val songName: String)
    companion object {
        lateinit var folderName: String
    }
    class ViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        val SongName: TextView = itemView.findViewById(R.id.songname)
        val deleteSong: TextView = itemView.findViewById(R.id.delete_song)
        val playsong : TextView = itemView.findViewById(R.id.play_song)
    }
    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.song_in_track_item, parent, false)
        return ViewHolder(view)
    }
    override fun getItemCount(): Int {
        return songList.size
    }
    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val currentItem = songList[position]

        holder.SongName.text = currentItem.songName

        holder.deleteSong.setOnClickListener {
            val currentItemSong = currentItem.songName
            Audio().deleteMusicInTrack(currentItemSong, folderName)
        }

        holder.playsong.setOnClickListener {
            val absolutepath = Audio().getAbsolutePathMp3File(currentItem.songName, folderName)
            val intent = Intent(holder.itemView.context, MainActivity::class.java)
            intent.putExtra("absolutepathsong", absolutepath)
            holder.itemView.context.startActivity(intent)
        }


    }


}