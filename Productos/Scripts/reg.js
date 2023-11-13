const logibox = document.querySelector('.logbox');
const loginLink = document.querySelector('.loginLink');
const regLink = document.querySelector('.regLink');

regLink.addEventListener('click', () => {
    logibox.classList.add('active');
});

loginLink.addEventListener('click', () => {
    logibox.classList.remove('active');
});