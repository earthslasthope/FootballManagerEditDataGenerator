import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import DataBuilderPage from './pages/data-builder.page';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={DataBuilderPage} />
      </Layout>
    );
  }
}
