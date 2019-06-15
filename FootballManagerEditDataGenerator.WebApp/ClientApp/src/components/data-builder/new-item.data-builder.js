import * as React from 'react';
import * as wikiApi from '../../api/wikipedia';

const NewItemContext = React.createContext({
    selectTeam: (teamName) => null,
    searchString: '',
    searchResults: null
});

const Reviewer = () => <NewItemContext.Consumer>
    {({reviewData}) => <div className="reviewer">
        {Object.keys(reviewData).map(key => reviewData[key]).map(({ property, values }) => <div key={property} className="review-item">
            <span className="property">{property}</span>
            <div className="values">
                {values.map(({ text, link, yearFrom, yearTo }, index) => <span key={index} className="value-item">
                    {text}
                </span>)}
            </div>
        </div>)}
    </div> }
</NewItemContext.Consumer>

const SearchResults = () => <NewItemContext.Consumer>
    {({searchResults, searchString, selectTeam}) => <div className="search-results">
        <span>Found matches for <span className="searchmatch">{searchString}</span></span>
        <ul>
            {searchResults.map(({pageTitle, snippetHtml}) => <li key={pageTitle} onClick={selectTeam.bind(this, pageTitle)}>
                <h6>{pageTitle}</h6>
                <p dangerouslySetInnerHTML={{ __html: snippetHtml }}></p>
            </li>)}
        </ul>
    </div> }
</NewItemContext.Consumer>

class NewItem extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            input: '',
            searchString: '',
            searchResults: null,
            reviewData: null
        };

        this.submit = this.submit.bind(this);
        this.selectTeam = this.selectTeam.bind(this);
    }

    submit(e) {
        e.preventDefault();
        const { input } = this.state;

        wikiApi.search(this.state.input).then(searchResults => {
            this.setState({
                searchResults,
                searchString: input,
                input: ''
            });
        });
    }

    selectTeam(teamName) {
        wikiApi.getInfobox(teamName).then(reviewData => {
            this.setState({
                reviewData,
                searchResults: null,
                searchString: ''
            });
        });
    }

    render() {
        const { selectTeam } = this;
        const { input, searchResults, searchString, reviewData } = this.state;

        return <NewItemContext.Provider value={{ selectTeam, searchResults, searchString, reviewData }}>
            <div className="new-item">
                <p>Enter name of team to search and review</p>
                <form onSubmit={this.submit}>
                    <input type="text" onChange={(e) => this.setState({ input: e.target.value })} value={input} />
                    <button type="submit" disabled={input.length === 0}>Search</button>
                </form>
                { searchResults && <SearchResults /> }
                { reviewData && <Reviewer /> }
            </div>
        </NewItemContext.Provider>
    }
}

export default NewItem;