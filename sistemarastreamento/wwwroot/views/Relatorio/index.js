async function btnReportVendaIndust() {
    var fim = new Date();
    var inicio = new Date();
    inicio.setDate(inicio.getDate() - 31)

    var dayInicio = ("0" + inicio.getDate()).slice(-2);
    var monthInicio = ("0" + (inicio.getMonth() + 1)).slice(-2);

    var dayFim = ("0" + fim.getDate()).slice(-2);
    var monthFim = ("0" + (fim.getMonth() + 1)).slice(-2);

    var datainicio = inicio.getFullYear() + "-" + (monthInicio) + "-" + (dayInicio);
    var datafim = fim.getFullYear() + "-" + (monthFim) + "-" + (dayFim);

    const { value: formValues } = await Swal.fire({
        title: 'Período para o relatório',
        html:
            '<label>Data Inicio</label>' +
            `<input type="date" id="swal-input1" class="swal2-input" value="${datainicio}">` +
            '<label>Data Fim</label>' +
            `<input type="date" id="swal-input2" class="swal2-input" value="${datafim}">`,
        showCloseButton: true,
        focusConfirm: true,
        confirmButtonText: 'Gerar Relatório',
        preConfirm: () => {
            return [
                document.getElementById('swal-input1').value,
                document.getElementById('swal-input2').value
            ]
        }

    })

    if (formValues) {
        var datainicio = formValues[0];
        var datafim = formValues[1];

        if (datainicio > datafim) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Data Inicio maior que a Data Fim!'
            })
        }
        else {
            Swal.fire({
                title: 'Gerando o Relatório!',
                html: 'aguarde!',
                onOpen: () => {
                    Swal.showLoading()
                }
            })

            var dados = {
                datainicio,
                datafim
            }

            var config = {
                method: "POST",
                headers: new Headers({
                    "Content-Type": "application/json"
                }),
                credentials: 'include', //inclui cookies
                body: JSON.stringify(dados)
            };

            fetch("/Relatorio/ReportVendaIndust", config)
                .then(function (retorno) {
                    retorno = retorno.blob();
                    return retorno;
                })
                .then(function (retorno) {
                    Swal.close()
                    var fileURL = URL.createObjectURL(retorno);
                    window.open(fileURL);
                })
                .catch(function () {
                    Swal.close()
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Erro, Tente Novamente!'
                    })
                })
        }

    }
} 

async function btnReportVendaDist() {

    var fim = new Date();
    var inicio = new Date();
    inicio.setDate(inicio.getDate() - 31)

    var dayInicio = ("0" + inicio.getDate()).slice(-2);
    var monthInicio = ("0" + (inicio.getMonth() + 1)).slice(-2);

    var dayFim = ("0" + fim.getDate()).slice(-2);
    var monthFim = ("0" + (fim.getMonth() + 1)).slice(-2);

    var datainicio = inicio.getFullYear() + "-" + (monthInicio) + "-" + (dayInicio);
    var datafim = fim.getFullYear() + "-" + (monthFim) + "-" + (dayFim);

    const { value: formValues } = await Swal.fire({
        title: 'Período para o relatório',
        html:
            '<label>Data Inicio</label>' +
            `<input type="date" id="swal-input1" class="swal2-input" value="${datainicio}">` +
            '<label>Data Fim</label>' +
            `<input type="date" id="swal-input2" class="swal2-input" value="${datafim}">`,
        showCloseButton: true,
        focusConfirm: true,
        confirmButtonText: 'Gerar Relatório',
        preConfirm: () => {
            return [
                document.getElementById('swal-input1').value,
                document.getElementById('swal-input2').value
            ]
        }

    })

    if (formValues) {
        var datainicio = formValues[0];
        var datafim = formValues[1];

        if (datainicio > datafim) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Data Inicio maior que a Data Fim!'
            })
        }
        else {

            Swal.fire({
                title: 'Gerando o Relatório!',
                html: 'aguarde!',
                onOpen: () => {
                    Swal.showLoading()
                }
            })

            var dados = {
                datainicio,
                datafim
            }

            var config = {
                method: "POST",
                headers: new Headers({
                    "Content-Type": "application/json"
                }),
                credentials: 'include', //inclui cookies
                body: JSON.stringify(dados)
            };

            fetch("/Relatorio/ReportVendaDist", config)
                .then(function (retorno) {
                    retorno = retorno.blob();
                    return retorno;
                })
                .then(function (retorno) {
                    Swal.close()
                    var fileURL = URL.createObjectURL(retorno);
                    window.open(fileURL);
                })
                .catch(function () {
                    Swal.close()
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Erro, Tente Novamente!'
                    })
                })
        }
        

    }
} 

async function btnReportProdIndust() {
    const { value: lote } = await Swal.fire({
        title: 'Filtro por Lote',
        input: 'text',
        inputLabel: 'Lote',
        inputPlaceholder: 'Lote',
        confirmButtonText: 'Gerar Relatório',
        showCloseButton: true,
        inputValidator: (value) => {
            if (!value) {
                return 'Informe o Lote!'
            }
        }
    })

    if (lote) {

        Swal.fire({
            title: 'Gerando o Relatório!',
            html: 'aguarde!',
            onOpen: () => {
                Swal.showLoading()
            }
        })

        var dados = {
            lote
        }

        var config = {
            method: "POST",
            headers: new Headers({
                "Content-Type": "application/json"
            }),
            credentials: 'include', //inclui cookies
            body: JSON.stringify(dados)
        };

        fetch("/Relatorio/ReportProdDist", config)
            .then(function (retorno) {
                retorno = retorno.blob();
                return retorno;
            })
            .then(function (retorno) {
                Swal.close()
                var fileURL = URL.createObjectURL(retorno);
                window.open(fileURL);
            })
            .catch(function () {
                Swal.close()
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Erro, Tente Novamente!'
                })
            })
    }
}

async function btnReportVendaDistribuidor() {

        const { value: cidade } = await Swal.fire({
        title: 'Filtro por Cidade',
        input: 'text',
        inputLabel: 'Cidade',
        inputPlaceholder: 'Cidade',
        confirmButtonText: 'Gerar Relatório',
        showCloseButton: true,
        inputValidator: (value) => {
            if (!value) {
                return 'Informe a Cidade!'
            }
        }
    })

    if (cidade) {

        Swal.fire({
            title: 'Gerando o Relatório!',
            html: 'aguarde!',
            onOpen: () => {
                Swal.showLoading()
            }
        })

        var dados = {
            cidade
        }

        var config = {
            method: "POST",
            headers: new Headers({
                "Content-Type": "application/json"
            }),
            credentials: 'include', //inclui cookies
            body: JSON.stringify(dados)
        };

        fetch("/Relatorio/ReportDistribuidor", config)
            .then(function (retorno) {
                retorno = retorno.blob();
                return retorno;
            })
            .then(function (retorno) {
                Swal.close()
                var fileURL = URL.createObjectURL(retorno);
                window.open(fileURL);
            })
            .catch(function () {
                Swal.close()
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Erro, Tente Novamente!'
                })
            })
    }
}