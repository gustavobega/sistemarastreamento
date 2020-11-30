var index = {

    btnImportar: function () {

        var carrega = document.getElementById("carrega");

        if (document.getElementById("xml").files.length == 0) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Selecione um Arquivo!'
            })
        }
        else {
            var file = document.getElementById("xml").files[0];

            var fd = new FormData();
            fd.append("fileXML", file);
            carrega.innerHTML = `<tr><td colspan="3"><img src=\"/img/ajax-loader.gif"\ /> verificando...</td></tr>`
            var configFD = {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                },
                body: fd
            };

            fetch("/LancamentoIndust/ImportXML", configFD)
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

                        //cadastro/altera no estoque também
                        var cnpjdist = dadosObj.cnpjdist;
                        var id_nota = dadosObj.id_nota;

                        var dados = {
                            cnpjdist   
                        }

                        var config2 = {
                            method: "POST",
                            headers: {
                                "Content-Type": "application/json; charset=utf-8",
                            },
                            credentials: 'include', //inclui cookies
                            body: JSON.stringify(dados)  //serializa
                        };

                        fetch("/LancamentoIndust/AlterarEstoque?id_nota=" + id_nota, config2)
                            .then(function (dadosJson) {
                                var obj = dadosJson.json();
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