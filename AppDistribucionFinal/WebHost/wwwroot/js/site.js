
const formulario = document.getElementById('formularioSubirImagen');

formulario.addEventListener('submit', async (e) => {
    e.preventDefault();

    const formData = new FormData(formulario);

    try {
        const respuesta = await fetch('/api/subir-imagen', {
            method: 'POST',
            body: formData
        });

        if (respuesta.ok) {
            alert('Archivo enviado con éxito');
        } else {
            alert('Hubo un error al enviar el archivo.');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Ocurrió un error en la conexión.');
    }
});