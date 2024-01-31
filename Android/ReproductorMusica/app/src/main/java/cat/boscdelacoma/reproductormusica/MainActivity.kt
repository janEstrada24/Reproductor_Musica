package cat.boscdelacoma.reproductormusica

import android.media.MediaPlayer
import android.os.Bundle
import android.os.Handler
import android.view.animation.AlphaAnimation
import android.view.animation.Animation
import android.widget.SeekBar
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.FragmentTransaction
import cat.boscdelacoma.reproductormusica.Apilogic.Canco
import java.io.File
import java.io.FileInputStream
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter
import java.util.Date

class MainActivity : AppCompatActivity() {
    private lateinit var botoPlayPause: TextView
    private var mediaPlayer: MediaPlayer = MediaPlayer()
    private lateinit var seekBarAudio: SeekBar
    private var isPlaying = false
    private val handler = Handler()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        val returnBtn : TextView = findViewById(R.id.back)
        val title : TextView = findViewById(R.id.songname)

        returnBtn.setOnClickListener(){
            finish()
        }

        initMainActivity()
        showFragments()
        changeSongName()
    }

    /**
     * Metode que ens ajuda a tornar al fragment anterior.
     * @return {Unit} No retorna res.
     * */
    fun tornarDesDeFragment() {
        supportFragmentManager.popBackStack()
    }
    fun changeSongName(){
    }
    /**
     * Metode que ens ajuda a inicialitzar la MainActivity.
     * @return {Unit} No retorna res.
     * */
    private fun initMainActivity() {

        botoPlayPause = findViewById(R.id.startSong)
        seekBarAudio = findViewById(R.id.progressBar)

        // TODO: Pendent de revisarl
        val absolutepathsong = intent.getStringExtra("absolutepathsong").toString()

        if (!absolutepathsong.isNullOrEmpty()) {
            mediaPlayer.setDataSource(absolutepathsong)
            mediaPlayer.prepare()
        }

        seekBarAudio.setOnSeekBarChangeListener(object : SeekBar.OnSeekBarChangeListener {
            override fun onProgressChanged(seekBar: SeekBar?, progress: Int, fromUser: Boolean) {
            }

            override fun onStartTrackingTouch(seekBar: SeekBar?) {
            }

            override fun onStopTrackingTouch(seekBar: SeekBar?) {
                seekBar?.let {
                    mediaPlayer.seekTo(it.progress)
                }
            }
        })

        botoPlayPause.setOnClickListener {
            if (mediaPlayer.isPlaying) {
                botoPlayPause.setBackgroundResource(R.drawable.playbtn)
                mediaPlayer.pause()
            } else {
                botoPlayPause.setBackgroundResource(R.drawable.stopbtn)
                mediaPlayer.start()
                seekBarAudio.max = mediaPlayer.duration
                updateSeekBar()
                postHistorial()
            }
        }
    }
    /**
     * Metode que ens ajuda a mostrar els fragments.
     * @return {Unit} No retorna res.
     * */
    private fun showFragments(){
        val AddSongToTrack: TextView = findViewById(R.id.AddSongToTrack)
        val addplaylist: TextView = findViewById(R.id.AddList)

        AddSongToTrack.setOnClickListener {
            val listOfSongsFragment = ListOfSongsFragment()
            val fragmentManager = supportFragmentManager
            val transaction: FragmentTransaction = fragmentManager.beginTransaction()
            val fadeIn: Animation = AlphaAnimation(0f, 1f)

            fadeIn.duration = 500 // Duración de la animación en milisegundos
            transaction.setCustomAnimations(R.anim.fade_in, R.anim.fade_out)
            transaction.replace(R.id.fragment_container, listOfSongsFragment)
            transaction.addToBackStack(null)
            transaction.commit()
        }
        addplaylist.setOnClickListener(){
            val trackName = TrackName()
            val fragmentManager = supportFragmentManager
            val transaction: FragmentTransaction = fragmentManager.beginTransaction()
            val fadeIn: Animation = AlphaAnimation(0f, 1f)

            fadeIn.duration = 500
            transaction.setCustomAnimations(R.anim.fade_in, R.anim.fade_out)
            transaction.replace(R.id.fragment_container, trackName)
            transaction.addToBackStack(null)
            transaction.commit()
        }
    }

    /**
     * Metode que ens ajuda a actualitzar la barra de la musca
     * @return {Unit} No retorna res.
     * */
    private fun updateSeekBar() {
        isPlaying = true
        handler.postDelayed(object : Runnable {
            override fun run() {
                if (isPlaying) {
                    seekBarAudio.progress = mediaPlayer.currentPosition
                    handler.postDelayed(this, 100) // Actualizar cada 100 milisegundos
                }
            }
        }, 100)
    }
    override fun onDestroy() {
        super.onDestroy()
        mediaPlayer.release()
    }


    fun postHistorial() {
        val formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'")
        val formattedDate = LocalDateTime.now().format(formatter).toString()
        HTTP_Mongo(this).postHistorialOfSongs("45","canço", formattedDate)

    }
}
