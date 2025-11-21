document.addEventListener('DOMContentLoaded', () => {
    // 1. Navegación pegajosa (Sticky Navbar)
    const header = document.querySelector('.header');
    let headerHeight = header.offsetHeight; // Obtener la altura del header

    function checkStickyNavbar() {
        if (window.scrollY > headerHeight) {
            header.classList.add('sticky');
        } else {
            header.classList.remove('sticky');
        }
    }

    // Ejecutar al cargar la página y al hacer scroll
    window.addEventListener('scroll', checkStickyNavbar);
    window.addEventListener('resize', () => {
        headerHeight = header.offsetHeight; // Recalcular en caso de resize
        checkStickyNavbar();
    });

    // 2. Carrusel / Dots de navegación (simulación)
    const carouselDots = document.querySelectorAll('.carousel-dots .dot');

    carouselDots.forEach(dot => {
        dot.addEventListener('click', () => {
            // Remover la clase 'active' de todos los dots
            carouselDots.forEach(d => d.classList.remove('active'));
            // Añadir la clase 'active' al dot clicado
            dot.classList.add('active');

            // Aquí es donde normalmente se cambiaría el contenido del carrusel.
            // Por ahora, solo es una simulación visual.
            console.log('Dot clicado. Si fuera un carrusel real, el contenido cambiaría aquí.');
        });
    });

    // 3. Animación de los círculos de estadísticas al hacer scroll (Ejemplo básico)
    // Usaremos Intersection Observer API para detectar cuando los elementos entran en la vista
    const statCircles = document.querySelectorAll('.stat-circle');

    const observerOptions = {
        root: null, // El viewport es el root
        rootMargin: '0px',
        threshold: 0.5 // El 50% del elemento debe estar visible
    };

    const observer = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate-in'); // Añade una clase para animar
                observer.unobserve(entry.target); // Deja de observar una vez animado
            }
        });
    }, observerOptions);

    statCircles.forEach(circle => {
        observer.observe(circle);
    });

    // 4. Interactividad de iconos de redes sociales
    const socialIcons = document.querySelectorAll('.social-icons a');
    const socialNetworks = {
        0: { name: 'Twitter', url: 'https://twitter.com' },
        1: { name: 'Facebook', url: 'https://facebook.com' },
        2: { name: 'Instagram', url: 'https://instagram.com' },
        3: { name: 'YouTube', url: 'https://youtube.com' }
    };

    socialIcons.forEach((icon, index) => {
        // Evento hover - mostrar tooltip
        icon.addEventListener('mouseenter', () => {
            const tooltip = document.createElement('div');
            tooltip.className = 'social-tooltip';
            tooltip.textContent = socialNetworks[index].name;
            icon.appendChild(tooltip);
        });

        icon.addEventListener('mouseleave', () => {
            const tooltip = icon.querySelector('.social-tooltip');
            if (tooltip) tooltip.remove();
        });

        // Evento click - abrir en nueva ventana y marcar como activo
        icon.addEventListener('click', (e) => {
            e.preventDefault();
            
            // Remover clase active de todos
            socialIcons.forEach(i => i.classList.remove('active'));
            
            // Agregar clase active al actual
            icon.classList.add('active');
            
            // Abrir en nueva ventana
            window.open(socialNetworks[index].url, '_blank');
            
            // Remover la clase active después de 2 segundos
            setTimeout(() => {
                icon.classList.remove('active');
            }, 2000);
        });

        // Animación de pulse al cargar
        setTimeout(() => {
            icon.style.animation = 'pulse-icon 1s ease-out';
        }, index * 200);
    });

    // 5. Avatar fallback: si la imagen no carga, mostrar iniciales
    const authorImgs = document.querySelectorAll('.author-avatar');
    authorImgs.forEach(img => {
        // Crear elemento fallback
        const alt = img.getAttribute('alt') || '';
        const initials = alt.split(' ').filter(Boolean).map(s => s[0]).join('').slice(0,2).toUpperCase() || '?';
        const fallback = document.createElement('div');
        fallback.className = 'avatar-fallback';
        fallback.textContent = initials;

        // Insertar fallback antes de la imagen
        img.parentNode.insertBefore(fallback, img);

        // Ocultar fallback inicialmente
        fallback.style.display = 'none';

        // Si la imagen falla al cargar
        img.addEventListener('error', () => {
            img.style.display = 'none';
            fallback.style.display = 'flex';
        });

        // Si la imagen carga correctamente, ocultar el fallback
        img.addEventListener('load', () => {
            fallback.style.display = 'none';
            img.style.display = 'block';
        });

        // Si ya está marcada como rota (por ejemplo al abrir desde file:), comprobar
        setTimeout(() => {
            if (img.naturalWidth === 0) {
                img.style.display = 'none';
                fallback.style.display = 'flex';
            }
        }, 100);
    });

        // 6. Autenticación simple (mostrar usuario y logout)
        function updateAuthUI(){
            const authEmail = localStorage.getItem('sdh_auth');
            const users = JSON.parse(localStorage.getItem('sdh_users') || '{}');
            const authButtonsList = document.querySelectorAll('.auth-buttons');

            authButtonsList.forEach(container => {
                if(authEmail && users[authEmail]){
                    const name = users[authEmail].name || authEmail.split('@')[0];
                    container.innerHTML = `
                        <span style="margin-right:10px;color:var(--text-color);font-weight:700;">Hola, ${escapeHtml(name)}</span>
                        <button id="logoutBtn" style="background:transparent;border:1px solid rgba(255,255,255,0.12);padding:6px 10px;border-radius:6px;color:var(--text-color);cursor:pointer;">Cerrar sesión</button>
                    `;
                    const btn = container.querySelector('#logoutBtn');
                    btn.addEventListener('click', () => {
                        localStorage.removeItem('sdh_auth');
                        // Opcional: redirigir a index
                        window.location.href = 'index.html';
                    });
                } else {
                    container.innerHTML = `
                        <a href="register.html"><i class="fas fa-user-circle"></i> Registrarse</a>
                        <span class="separator">|</span>
                        <a href="login.html">Iniciar sesión</a>
                    `;
                }
            });
        }

        // Pequeña función para escapar html en el nombre
        function escapeHtml(str){
            return String(str).replace(/[&<>"'`]/g, s => ({
                '&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":"&#39;","`":"&#96;"
            })[s]);
        }

        // Ejecutar al cargar
        updateAuthUI();
});