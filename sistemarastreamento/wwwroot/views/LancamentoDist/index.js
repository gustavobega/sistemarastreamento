var index = {

    btnImportar: function () {

        document.getElementById("visualizador").style.display = 'none';

        var carrega = document.getElementById("carrega");
        
        var file = document.getElementById("xml").files[0];

        var fd = new FormData();
        fd.append("fileXML", file);

        if (document.getElementById("xml").files.length == 0) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Selecione um Arquivo!'
            })
        }
        else {
            carrega.innerHTML = `<tr><td colspan="3"><img src=\"/img/ajax-loader.gif"\ /> verificando...</td></tr>`
            var configFD = {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                },
                body: fd
            };

            fetch("/LancamentoDist/ImportXML", configFD)
                .then(function (dadosJson) {
                    var obj = dadosJson.json(); //deserializando
                    return obj;
                })
                .then(function (dadosObj) {
                    if (dadosObj.operacao) { 

                        Swal.fire({
                            position: 'center',
                            icon: 'success',
                            title: dadosObj.msg,
                            showConfirmButton: false,
                            timer: 1500
                        })

                        document.getElementById("visualizador").innerHTML = `
                                    <button type="button" class="btnVisualizar" data-fancybox data-type="iframe" data-src="/LancamentoDist/IndexVisualizar?id=${dadosObj.id_nota}">
                                        <span class="button-text">Visualizar</span>
                                    </button>
                                    `
                        document.getElementById("visualizador").style.display = 'block';
                        document.getElementById("info").innerHTML = dadosObj.infoadicionais;

                        var config = {
                            method: "POST",
                            headers: {
                                "Accept": "application/json",
                            },
                        };
                        fetch("/LancamentoDist/RetiraEstoque?id_nota=" + dadosObj.id_nota, config)
                            .then(function (dadosJson) {
                                var obj = dadosJson.json(); //deserializando
                                return obj;
                            })
                            .then(function (dadosObj) {

                            })

                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: dadosObj.msg
                        })
                    }
                    
                })
                .catch(function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Algo deu Errado, Tente Novamente!'
                    })
                })
                .finally(function () {
                    carrega.innerHTML = "";
                });

        }
        
    }

}