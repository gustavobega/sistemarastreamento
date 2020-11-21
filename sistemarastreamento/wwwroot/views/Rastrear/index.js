var index = {
    BtnRastrear: function () {
        var lote = document.getElementById('lote').value
        var codigo = document.getElementById('codigo').value
        var hospital = document.getElementById('hospital').value

        if (codigo.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Informe o Código!'
            })
        }
        else if (lote.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Informe o Lote!'
            })
        }
        else if (hospital.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Informe o Hospital!'
            })
        }
        else {
            document.getElementById('btnbuscar').value = 'aguarde...'
            document.getElementById('btnbuscar').style.background = '#5092c8'
            document.getElementById('gif-login').style.display = 'block'
            document.getElementById('dadosbusca').innerHTML = ''
            var dados = {
                lote,
                codigo,
                hospital
            }

            var config = {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                },
                credentials: 'include', //inclui cookies
                body: JSON.stringify(dados)  //serializa
            };

            fetch("/Rastrear/Buscar", config)
                .then(function (dadosJson) {
                    var obj = dadosJson.json(); //deserializando
                    return obj;
                })
                .then(function (dadosObj) {
                    if (dadosObj.operacao) {
                        const Toast = Swal.mixin({
                            toast: true,
                            position: 'top-end',
                            showConfirmButton: false,
                            timer: 3000,
                            timerProgressBar: true,
                            didOpen: (toast) => {
                                toast.addEventListener('mouseenter', Swal.stopTimer)
                                toast.addEventListener('mouseleave', Swal.resumeTimer)
                            }
                        })

                        Toast.fire({
                            icon: 'success',
                            title: 'Item encontrado com sucesso!'
                        })
                        //printa buscar
                        document.getElementById('dadosbusca').innerHTML = `
                            <div class="descricao">
                                <h2>Descrição do Item</h2>
                                <p>${dadosObj.rastreioDestino.descricao.toUpperCase()}</p>
                            </div>
                            <hr />
                            <div>
                                <h2>Dados Origem</h2>
                                <p>EMPRESA: ${dadosObj.indust.nome.toUpperCase()}</p>
                                <p>ENDEREÇO: ${dadosObj.indust.rua.toUpperCase()}, ${dadosObj.indust.numero}, ${dadosObj.indust.bairro.toUpperCase()}</p>
                                <p>${dadosObj.cidadeIndust.toUpperCase()} - ${dadosObj.estadoIndust.toUpperCase()}</p>
                            </div>
                            <hr />
                            <div>
                                <h2>Dados do Distribuidor</h2>
                                <p>EMPRESA: ${dadosObj.dist.nome.toUpperCase()}</p>
                                <p>ENDEREÇO: ${dadosObj.dist.rua.toUpperCase()}, ${dadosObj.dist.numero}, ${dadosObj.dist.bairro.toUpperCase()}</p>
                                <p>${dadosObj.cidadeDist.toUpperCase()} - ${dadosObj.estadoDist.toUpperCase()}</p>
                            </div>
                            <hr />
                            <div>
                                <h2>Dados do Destino</h2>
                                <p>HOSPITAL: ${dadosObj.rastreioDestino.rdest_nome.toUpperCase()}</p> 
                                <p>ENDEREÇO: ${dadosObj.rastreioDestino.rdest_rua.toUpperCase()}, ${dadosObj.rastreioDestino.rdest_numero}, ${dadosObj.rastreioDestino.rdest_bairro.toUpperCase()}</p> 
                                <p>${dadosObj.rastreioDestino.rdest_cidade.toUpperCase()} - ${dadosObj.rastreioDestino.rdest_estado.toUpperCase()}</p> 
                            </div>  
                        `
                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Item não encontrado, reveja os campos!'
                        })
                        document.getElementById('dadosbusca').innerHTML = '<h5>aguardando...</h5>'
                    }
                    document.getElementById('btnbuscar').value = 'Buscar'
                    document.getElementById('gif-login').style.display = 'none'
                    document.getElementById('btnbuscar').style.background = '#4682B4'

                });
        } 
    }
}

document.getElementById("lote")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            index.BtnRastrear();
        }
    });

document.getElementById("codigo")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            index.BtnRastrear();
        }
    });

document.getElementById("hospital")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            index.BtnRastrear();
        }
    });