const openMenu = document.querySelector('#show-menu')
const hideMenuIcon = document.querySelector('#hide-menu')
const sideMenu = document.querySelector('#nav-menu')

openMenu.addEventListener('click', function () {
    sideMenu.classList.add('active')
    $(window).bind('scroll', setTopo); 

    document.getElementById('contato').style.opacity = 0.6;
    document.getElementById('contato').style.pointerEvents = 'none'

    document.getElementById('navbar').style.opacity = 0.6;
    document.getElementById('navbar').style.pointerEvents = 'none'

    document.getElementById('footer').style.opacity = 0.6;
    document.getElementById('footer').style.pointerEvents = 'none'

    document.getElementById('section').style.opacity = 0.6;
    document.getElementById('section').style.pointerEvents = 'none'
})

hideMenuIcon.addEventListener('click', function () {
    sideMenu.classList.remove('active')
    $(window).unbind('scroll', setTopo);
    document.getElementById('contato').style.opacity = 1;
    document.getElementById('contato').style.pointerEvents = 'visible'

    document.getElementById('navbar').style.opacity = 1;
    document.getElementById('navbar').style.pointerEvents = 'visible'

    document.getElementById('footer').style.opacity = 1;
    document.getElementById('footer').style.pointerEvents = 'visible'

    document.getElementById('section').style.opacity = 1;
    document.getElementById('section').style.pointerEvents = 'visible'
})

function setTopo() {
    $(window).scrollTop(0);
}