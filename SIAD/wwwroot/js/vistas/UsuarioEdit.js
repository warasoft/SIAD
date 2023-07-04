//// Call the dataTables jQuery plugin


//function UsuarioDetalle(UsuarioId) {
//    $.ajax({
//        cache: false,
//        url: "/Usuario/GetUsuario/",
//        data: { UsuarioId },
//        dataType: "html",
//        type: "post",
//        success: function (result) {
//            $(#modalEdit).html(result);
//        }
//    })
//}


function obtenerUsuario(idUsuario) {
    // Realizar la solicitud AJAX al controlador
    $.ajax({
        url: `/Usuario/ObtenerUsuario?id=${idUsuario}`,
        method: 'GET',
        success: function (data) {
            // Manejar los datos devueltos por el controlador
            mostrarModalUsuario(data);
        },
        error: function (error) {
            // Manejar errores de la solicitud AJAX
            console.error(error);
        }
    });
}

//function mostrarModalUsuario(usuario) {
//    // Obtener el elemento del modal
//    var modal = $('#modalEdit');
//    // Actualizar los datos del usuario en el modal
//    modal.find('.modal-title').text(usuario.nombre);
//    modal.find('.modal-body #matricula').text(usuario.matricula);
//    // Abrir el modal
//    modal.modal('show');
//}

function mostrarModalUsuario(usuario) {
    // Obtener el elemento del modal y los elementos donde mostrar los datos
    var modal = document.getElementById('modalEdit');
    var matricula = document.getElementById('matricula');
    var grado = document.getElementById('grado');
    var apellido = document.getElementById('apellido');
    var nombre = document.getElementById('nombre');
    var destino = document.getElementById('destino');
    var deptoDiv = document.getElementById('deptoDiv');
    var phoneNumber = document.getElementById('phoneNumber');

    // Mostrar los datos del usuario en los elementos correspondientes
    matricula.value = usuario.matricula;
    grado.value = usuario.grado;
    apellido.value = usuario.apellido;
    nombre.value = usuario.nombre;
    destino.value = usuario.destino;
    deptoDiv.value = usuario.deptoDiv;
    phoneNumber.value = usuario.phoneNumber;
    // Abrir la ventana modal
    $(modal).modal('show');
}
