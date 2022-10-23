import React, { useState, useEffect } from 'react';
import axios from 'axios';

const ViewAll = () => {

    const [jokes, setJokes] = useState([]);

    useEffect(() => {
        const getJokes = async () => {
            const { data } = await axios.get('/api/Joke/getJokes');
            setJokes(data);
        }

        getJokes();
    }, [])

    const jokeBox = (joke, key) => {
        return <div className='clo-md-6 offset-md-3' key={key}>
            <div className="card card-body bg-light mb-3" >
                <h5>{joke.setup}</h5>
                <h5>{joke.punchline}</h5>
                <span>Likes: {joke.userLikedJokes.filter(x => x.liked === true).length}</span>
                <span>Dislikes: {joke.userLikedJokes.filter(x => x.liked === false).length}</span>
            </div>
        </div >
    }

    return (jokes.map((j, k) => jokeBox(j, k)))
}
export default ViewAll;