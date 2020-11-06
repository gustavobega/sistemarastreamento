function openmenu() {
    var windowWidth = window.innerWidth;

    if (windowWidth > 968) {

        document.getElementById('habilitaCad').style.top = '70px';

        if (document.getElementById('habilitaCad').style.opacity == 0) {
            document.getElementById('habilitaCad').style.opacity = 1;
            document.getElementById('habilitaCad').style.visibility = 'visible';

            //esconde o menu relatorio
            document.getElementById('habilitaRel').style.opacity = 0;
            document.getElementById('habilitaRel').style.visibility = 'hidden';

            //esconde o user
            document.getElementById('habilitaUser').style.opacity = 0;
            document.getElementById('habilitaUser').style.visibility = 'hidden';
        }
        else {
            document.getElementById('habilitaCad').style.opacity = 0;
            document.getElementById('habilitaCad').style.visibility = 'hidden';
        }

    }
}

function openRel() {
    var windowWidth = window.innerWidth;

    if (windowWidth > 968) {
        document.getElementById('habilitaRel').style.top = '70px';

        if (document.getElementById('habilitaRel').style.opacity == 0) {
            document.getElementById('habilitaRel').style.opacity = 1;
            document.getElementById('habilitaRel').style.visibility = 'visible';

            //esconde o menu cadastro
            document.getElementById('habilitaCad').style.opacity = 0;
            document.getElementById('habilitaCad').style.visibility = 'hidden';

            //esconde o user
            document.getElementById('habilitaUser').style.opacity = 0;
            document.getElementById('habilitaUser').style.visibility = 'hidden';
        }
        else {
            document.getElementById('habilitaRel').style.opacity = 0;
            document.getElementById('habilitaRel').style.visibility = 'hidden';
        }
    }
}

function openUser() {
    var windowWidth = window.innerWidth;

    if (windowWidth > 968) {
        document.getElementById('habilitaUser').style.top = '70px';

        if (document.getElementById('habilitaUser').style.opacity == 0) {
            document.getElementById('habilitaUser').style.opacity = 1;
            document.getElementById('habilitaUser').style.visibility = 'visible';

            //esconde o menu cadastro
            document.getElementById('habilitaCad').style.opacity = 0;
            document.getElementById('habilitaCad').style.visibility = 'hidden';

            //esconde o menu relatorio
            document.getElementById('habilitaRel').style.opacity = 0;
            document.getElementById('habilitaRel').style.visibility = 'hidden';

        }
        else {
            document.getElementById('habilitaUser').style.opacity = 0;
            document.getElementById('habilitaUser').style.visibility = 'hidden';
        }
    }
}