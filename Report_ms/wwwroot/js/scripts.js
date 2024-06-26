document.addEventListener('DOMContentLoaded', async function() {
    const form = document.getElementById('report-form');
    const searchForm = document.getElementById('search-form');
    const reportsList = document.getElementById('reports-list');
    const searchResult = document.getElementById('search-result');

    // Función para cargar los informes al cargar la página
    async function loadReports() {
        reportsList.innerHTML = ''; // Limpiar la lista de informes antes de actualizarla
        try {
            const response = await fetch('/api/report');
            if (!response.ok) {
                throw new Error('Error al obtener los informes.');
            }
            const reports = await response.json();
            reports.forEach(report => {
                renderReport(report);
            });
        } catch (error) {
            console.error('Error al cargar los informes:', error);
            reportsList.innerHTML = '<li>Error al cargar los informes. Inténtalo de nuevo más tarde.</li>';
        }
    }

    // Función para renderizar un informe en la lista
    function renderReport(report) {
        const li = document.createElement('li');
        li.innerHTML = `
            <strong>ID:</strong> ${report._id}<br>
            <strong>Fecha:</strong> ${new Date(report.date).toLocaleString()}<br>
            <strong>Observación:</strong> ${report.observation}<br>
            <strong>ID de Usuario:</strong> ${report.userId}<br>
            <strong>ID de Medición:</strong> ${report.measurementId}<br>
            <strong>ID de Alerta:</strong> ${report.alertId}<br>
            <button class="edit" onclick="editReport('${report._id}')">Editar</button>
            <button class="delete" onclick="deleteReport('${report._id}')">Eliminar</button>
            <hr>
        `;
        reportsList.appendChild(li);
    }

    // Función para manejar el envío del formulario (crear o actualizar informe)
    form.addEventListener('submit', async function(event) {
        event.preventDefault();
        
        const formData = new FormData(form);
        const id = formData.get('id');
        const url = id ? `/api/report/${id}` : '/api/report';
        const method = id ? 'PUT' : 'POST';

        try {
            const response = await fetch(url, {
                method: method,
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    date: formData.get('date'),
                    observation: formData.get('observation'),
                    userId: formData.get('userId'),
                    measurementId: formData.get('measurementId'),
                    alertId: formData.get('alertId')
                })
            });

            if (!response.ok) {
                throw new Error('Error al guardar el informe.');
            }

            // Limpiar el formulario después de guardar
            form.reset();

            // Recargar la lista de informes
            loadReports();

            // Mostrar mensaje de éxito
            alert('Informe guardado correctamente.');

        } catch (error) {
            console.error('Error al guardar el informe:', error);
            alert('Error al guardar el informe. Inténtalo de nuevo más tarde.');
        }
    });

    // Función para editar un informe
    window.editReport = async function(id) {
        try {
            const response = await fetch(`/api/report/${id}`);
            if (!response.ok) {
                throw new Error('Error al obtener el informe.');
            }
            const report = await response.json();

            // Rellenar el formulario con los datos del informe
            document.getElementById('report-id').value = report._id;
            document.getElementById('date').value = new Date(report.date).toISOString().slice(0, 16); // Formato datetime-local
            document.getElementById('observation').value = report.observation;
            document.getElementById('userId').value = report.userId;
            document.getElementById('measurementId').value = report.measurementId;
            document.getElementById('alertId').value = report.alertId;

        } catch (error) {
            console.error('Error al obtener el informe:', error);
            alert('Error al obtener el informe. Inténtalo de nuevo más tarde.');
        }
    };

    // Función para eliminar un informe
    window.deleteReport = async function(id) {
        if (!confirm('¿Estás seguro de que deseas eliminar este informe?')) {
            return;
        }

        try {
            const response = await fetch(`/api/report/${id}`, {
                method: 'DELETE'
            });
            if (!response.ok) {
                throw new Error('Error al eliminar el informe.');
            }

            // Actualizar la lista de informes después de eliminar
            loadReports();

            // Mostrar mensaje de éxito
            alert('Informe eliminado correctamente.');

        } catch (error) {
            console.error('Error al eliminar el informe:', error);
            alert('Error al eliminar el informe. Inténtalo de nuevo más tarde.');
        }
    };

    // Función para buscar un informe por ID
    searchForm.addEventListener('submit', async function(event) {
        event.preventDefault();
        
        const searchId = document.getElementById('search-id').value;

        try {
            const response = await fetch(`/api/report/${searchId}`);
            if (!response.ok) {
                throw new Error('Informe no encontrado.');
            }
            const report = await response.json();
            searchResult.innerHTML = `
                <strong>ID:</strong> ${report._id}<br>
                <strong>Fecha:</strong> ${new Date(report.date).toLocaleString()}<br>
                <strong>Observación:</strong> ${report.observation}<br>
                <strong>ID de Usuario:</strong> ${report.userId}<br>
                <strong>ID de Medición:</strong> ${report.measurementId}<br>
                <strong>ID de Alerta:</strong> ${report.alertId}<br>
                <button onclick="editReport('${report._id}')">Editar</button>
                <button onclick="deleteReport('${report._id}')">Eliminar</button>
            `;

        } catch (error) {
            console.error('Error al buscar el informe:', error);
            searchResult.innerHTML = 'Informe no encontrado.';
        }
    });

    // Llamar a loadReports al cargar la página inicialmente
    loadReports();
});
