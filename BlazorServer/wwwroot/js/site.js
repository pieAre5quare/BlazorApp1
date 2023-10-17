window.PlayAudioFileStream = async (contentStreamReference) => {

    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);

    var sound = document.getElementById('soundTest');
    sound.src = url;
    sound.type = 'audio/mpeg';
    sound.style.visibility = 'visible';
    //document.getElementById('audioContainer').appendChild(sound);
    sound.load();
    //sound.play();
    //sound.onended = function () {
    //    document.body.removeChild(sound);
    //    URL.revokeObjectURL(url);
    //};
}