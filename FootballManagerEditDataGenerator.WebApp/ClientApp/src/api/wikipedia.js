import axios from 'axios';

const http = axios.create({ 
    baseURL: '/' // TODO - Replace with something from SPA services
});

export function getInfobox(pageTitle) {
    return http.get('api/wikipedia/infobox/' + pageTitle).then(response => response.data);
}

export function search(searchString) {
    return http.get('api/wikipedia/search/' + searchString).then(response => response.data);
}